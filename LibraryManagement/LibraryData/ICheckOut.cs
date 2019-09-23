using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ICheckOut
    {
        void Add(CheckOut newCheckout);
        IEnumerable<CheckOut> GetAll();
        IEnumerable<CheckoutHistory> GetCheckoutHistories(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);


        CheckOut GetById(int checkoutId);
        CheckOut GetLatestCheckOut(int checkOutId);
        string GetCurrentHoldPattronName(int id);
        string GetCurrentCheckOutPattron(int assetId);
        DateTime GetCurrentHoldPlaced(int id);


        bool IsCheckeOut(int assetId);
        void CheckOutItem(int assetId, int LibraryCardId);
        void CheckInItem(int assetId);
        void PlaceHold(int assetId, int libraryCardId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);
        
        
        
        
       
    }
}
