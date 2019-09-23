using LibraryData;
using LibraryManagement.Models.Catalog;
using LibraryManagement.Models.CheckOutModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class CatalogController : Controller
    {
        private ILibraryAsset _asset;
        private ICheckOut _checkOut;

        public CatalogController(ILibraryAsset asset,ICheckOut checkOut)
        {
            _asset = asset;
            _checkOut = checkOut;
        }
        public IActionResult Index()
        {
            var assetModels = _asset.GetAll();
            var listingResult = assetModels.Select(result => new AssetIndexListingModels
            {
                Id = result.Id,
                ImageUrl = result.ImageUrl,
                AuthorOrDirector = _asset.GetAuthorOrDirector(result.Id),
                DeweyCallNumber = _asset.GetDeweyIndex(result.Id),
                Title = result.Title,
                Type = _asset.GetType(result.Id)
            });
            var model = new AssetIndexModel()
            {
                Assets = listingResult
            };
            return View(model);
        }
        public IActionResult Detail(int id)
        {
            var asset = _asset.GetBYId(id);

            var curretnHolds = _checkOut.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced = _checkOut.GetCurrentHoldPlaced(a.Id).ToString("d"),
                    PatronName = _checkOut.GetCurrentHoldPattronName(a.Id)
                });

            
            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _asset.GetAuthorOrDirector(id),
                CurrentLocation = _asset.GetCurrentLocatuon(id).Name,
                DeweyCallNumber = _asset.GetDeweyIndex(id),
                CheckoutHistories = _checkOut.GetCheckoutHistories(id),
                ISBN = _asset.GetIsbn(id),
                LatestCheckOut = _checkOut.GetLatestCheckOut(id),
                PatronName=_checkOut.GetCurrentCheckOutPattron(id),
                CurrentHolds=curretnHolds
            };
            return View(model);
        }

        public IActionResult Checkout(int id)
        {
            var asset = _asset.GetBYId(id);
            var model = new CheckOutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckOut = _checkOut.IsCheckeOut(id)
            };
            return View(model);
        }
        public IActionResult CheckIn( int id)
        {
            _checkOut.CheckInItem(id);
            return RedirectToAction("Detail", new { id = id });
        }

        public IActionResult Hold(int id)
        {
            var asset = _asset.GetBYId(id);
            var model = new CheckOutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckOut = _checkOut.IsCheckeOut(id),
                HoldCount = _checkOut.GetCurrentHolds(id).Count()
            };
            return View(model);
        }
        public IActionResult MarkLost(int assetId)
        {
            _checkOut.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        public IActionResult MarkFound(int assetId)
        {
            _checkOut.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        
        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId)
        {

            _checkOut.CheckOutItem(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkOut.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }
    }
}
