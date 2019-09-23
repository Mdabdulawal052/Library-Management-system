using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    
   public class CheckOutService : ICheckOut
    {
        private LibraryContext _context;
        public CheckOutService( LibraryContext context)
        {
            _context = context;
        }
        public void Add(CheckOut newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

       

        public IEnumerable<CheckOut> GetAll()
        {
            return _context.CheckOuts;
        }

        public CheckOut GetById(int checkoutId)
        {
            return GetAll().FirstOrDefault(checkOut => checkOut.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistories(int id)
        {
            return _context.CheckoutHistories
                .Include(h=>h.LibraryAsset)
                .Include(h => h.LlibraryCard)
                .Where(h => h.LibraryAsset.Id == id);
        }

       

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(c => c.LibraryAsset)
                .Where(c => c.LibraryAsset.Id == id);
        }

        public CheckOut GetLatestCheckOut(int assetId)
        {
            return _context.CheckOuts.Where(c => c.LibraryAsset.Id == assetId)
                .OrderByDescending(c=>c.Since)
                .FirstOrDefault();
        }
        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;

            UpdateAssetStatus(assetId, "Available");
            //remove any existing checkout on the item
            RemoveExistingCheckouts(assetId);

            //close any existing checkout history
            ClosingExistingCheckOutHistory(assetId, now);
            

            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int assetId, string v)
        {
            var item = _context.LibraryAssets.FirstOrDefault(a => a.Id == assetId);
            _context.Update(item);
            item.status = _context.Statuses.FirstOrDefault(status => status.Name == v);
        }

        private void ClosingExistingCheckOutHistory(int assetId, DateTime now)
        {
            //close any existing checkout history
            var history = _context.CheckoutHistories
                .FirstOrDefault(c => c.LibraryAsset.Id == assetId && c.CheckedIn == null);
            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void RemoveExistingCheckouts(int assetId)
        {
            //remove any existing checkout on the item
            var checkout = _context.CheckOuts.FirstOrDefault(ch => ch.LibraryAsset.Id == assetId);
            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {
            UpdateAssetStatus(assetId, "Lost");
            
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId)
        {
            var now = DateTime.Now;
            var item = _context.LibraryAssets.FirstOrDefault(a => a.Id == assetId);
            
            // remove any existung checkouts on the item
            RemoveExistingCheckouts(assetId);
            // close any existing checkout hiistory
            ClosingExistingCheckOutHistory(assetId, now);
            // look for existing holds on the item
            var currentHolds = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LlibraryCard)
                .Where(h => h.LibraryAsset.Id == assetId);
            // if there are holds, checkout the item to the 
            //    librarycard with the earliest hold.
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);
                return;

            }
            // Otherwise, update the item status to available
            UpdateAssetStatus(assetId, "Available");
            _context.SaveChanges();

        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
          
            var earliestHold = currentHolds.OrderBy(holds => holds.HoldPlaced)
                .FirstOrDefault();
            var card = earliestHold.LlibraryCard;
            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckOutItem(assetId, card.Id);
        }

        public void CheckOutItem(int assetId, int LibraryCardId)
        {
            if(IsCheckOut(assetId))
            {
                return;
            }
            var item = _context.LibraryAssets.FirstOrDefault(a => a.Id == assetId);
            //This is function where status update
            UpdateAssetStatus(assetId, "CheckedOut");
            var libraryCard = _context.LibraryCards
                .Include(card => card.CheckOuts)
                .FirstOrDefault(card => card.Id == LibraryCardId);
            var now = DateTime.Now;
            if (libraryCard != null)
            {
                var checkOut = new CheckOut
                {
                    LibraryAsset = item,
                    LibraryCard = libraryCard,
                    Since = now,
                    Untill = GetDefaultCheckOutTime(now)
                };
                _context.Add(checkOut);
                var checkOutHistory = new CheckoutHistory
                {
                    CheckedOut = now,
                    LibraryAsset = item,
                    LlibraryCard = libraryCard
                };
                _context.Add(checkOutHistory);
                _context.SaveChanges();
            }
            
            
        }

        private DateTime GetDefaultCheckOutTime(DateTime now)
        {
            return now.AddDays(30);
        }
        public bool IsCheckeOut(int assetId)
        {
            return _context.CheckOuts
                .Where(co => co.LibraryAsset.Id == assetId).Any();
        }

        private bool IsCheckOut(int assetId)
        {
            return _context.CheckOuts
                .Where(co => co.LibraryAsset.Id == assetId)
                .Any();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;
            var asset = _context.LibraryAssets
                .Include(a=>a.status)
                .FirstOrDefault(a => a.Id == assetId);
            var card = _context.LibraryCards.FirstOrDefault(c => c.Id == libraryCardId);
            if (asset.status.Name == "Available")
            {
                UpdateAssetStatus(assetId, "On Hold");
            }
            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LlibraryCard = card
            };
            _context.Add(hold);
            _context.SaveChanges();
        }
       
        public string GetCurrentHoldPattronName(int holdId)
        {
            var hold = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LlibraryCard)
                .FirstOrDefault(h => h.Id == holdId);
            var cardId = hold?.LlibraryCard.Id;
            var parton = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);
            return parton?.FirstName + " " + parton?.LastName;
        }

        public DateTime GetCurrentHoldPlaced(int holdId)
        {
            return _context.Holds
                 .Include(h => h.LibraryAsset)
                 .Include(h => h.LlibraryCard)
                 .FirstOrDefault(h => h.Id == holdId).HoldPlaced;
        }

        public string GetCurrentCheckOutPattron(int assetId)
        {
            var checkOut = GetCheckOutByAssetId(assetId);
            if(checkOut== null)
            {
                return "";
            }
            var cardId = checkOut.LibraryCard.Id;
            var pattron = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return pattron.FirstName + " " + pattron.LastName;

        }

        private CheckOut GetCheckOutByAssetId(int assetId)
        {
            return _context.CheckOuts
                .Include(co => co.LibraryAsset)
                .Include(co => co.LibraryCard)
                .FirstOrDefault(co => co.LibraryAsset.Id == assetId);
        }

        
    }
}
