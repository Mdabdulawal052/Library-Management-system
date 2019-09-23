using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class PatronService : IPatron
    {
        private LibraryContext _context;
        public PatronService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Patron newPatron)
        {
            _context.Add(newPatron);
            _context.SaveChanges();
        }

        public Patron Get(int id)
        {
            return GetAll()
                .FirstOrDefault(patron => patron.Id == id);

            //***We Can use GetAll Instead of following Code
            //   because they are same

            //return _context.Patrons
            //    .Include(patron => patron.LibraryCard)
            //    .Include(patron => patron.HomeLibraryBranch)
            //    .FirstOrDefault(patron => patron.Id == id);
        }

        public IEnumerable<Patron> GetAll()
        {
            return _context.Patrons
                .Include(patron => patron.LibraryCard)
                .Include(patron => patron.HomeLibraryBranch);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistories(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;
            //**Same so 

            //var cardId= _context.Patrons
            //   .Include(patron => patron.LibraryCard)
            //  .FirstOrDefault(patron => patron.Id == patronId)
            //  .LibraryCard.Id;

            return _context.CheckoutHistories
                .Include(co => co.LlibraryCard)
                .Include(co => co.LibraryAsset)
                .Where(co => co.LlibraryCard.Id == cardId)
                .OrderByDescending(co => co.CheckedOut);
        }

        public IEnumerable<CheckOut> GetCheckOuts(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;

            return _context.CheckOuts
                .Include(co => co.LibraryCard)
                .Include(co => co.LibraryAsset)
                .Where(co => co.LibraryCard.Id == cardId);
        }

        public IEnumerable<Hold> GetHolds(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;

            return _context.Holds
                .Include(h => h.LlibraryCard)
                .Include(h => h.LibraryAsset)
                .Where(h => h.LlibraryCard.Id == cardId)
                .OrderByDescending(h=>h.HoldPlaced);
        }
    }
}
