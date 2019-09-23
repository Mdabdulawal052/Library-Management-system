using LibraryData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;


namespace LibraryManagement.Models.Patron
{
    public class PatronDetailModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public int LibraryCardId { get; set; }
        public int HomeLibraryBranchId { get; set; }
        public string Address { get; set; }
        public DateTime MemberSince { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Fees { get; set; }
        public string TelePhoneNumber { get; set; }
        public string HomeLibraryBranch { get; set; }
        public decimal OverduFees { get; set; }

        public IEnumerable<CheckOut> AssetsCheckedOut { get; set; }
        public IEnumerable<CheckoutHistory> CheckOutHistroy { get; set; }
        public IEnumerable<Hold> Holds { get; set; }
        public IEnumerable<SelectListItem> LibraryBranchesList { get; set; }
        public IEnumerable<SelectListItem> LibraryCardList { get; set; }

    }
}
