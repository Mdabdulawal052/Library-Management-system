using LibraryData;
using LibraryData.Models;
using LibraryManagement.Models.Patron;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class PatronController:Controller
    {
        private readonly LibraryContext _context;
        private IPatron _patron;

        public PatronController(IPatron patron, LibraryContext context)
        {
            _context = context;
            _patron = patron;
        }

        public IActionResult Index()
        {
            var allPatrons = _patron.GetAll();
            var patronModels = allPatrons.Select(p => new PatronDetailModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                LibraryCardId = p.LibraryCard.Id,
                OverduFees = p.LibraryCard.Fees,
                HomeLibraryBranch =p.HomeLibraryBranch.Name
            }).ToList();


            var model = new PatronIndexModel()
            {
                Patrons = patronModels
            };
            return View(model);
        }
        public IActionResult Create()
        {
            PatronDetailModel patronDetailModel = new PatronDetailModel();
           
            patronDetailModel.LibraryBranchesList = _context.LibraryBranches.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            return View(patronDetailModel);
        }
        [HttpPost]
        public IActionResult Create(PatronDetailModel patronDetailModel)
        {

            var duplicate = _context.Patrons.Any(p => p.FirstName == patronDetailModel.FirstName && p.LastName == patronDetailModel.LastName && p.DateOfBirth == patronDetailModel.DateOfBirth);
            if (duplicate)
            {
                var date = DateTime.Now;
                LibraryCard libraryCard = new LibraryCard()
                {
                    Fees = patronDetailModel.Fees,
                    FirstName = patronDetailModel.FirstName,
                    LastName = patronDetailModel.LastName,
                    Created = date
                };
                _context.Add(libraryCard);
                _context.SaveChanges();
                var patronLibrarycard = _context.LibraryCards.Where(l => l.FirstName == patronDetailModel.FirstName &&
                                  l.LastName == patronDetailModel.LastName && l.Created == date).FirstOrDefault();
                if (patronLibrarycard != null)
                {

                    Patron patron = new Patron()
                    {
                        FirstName = patronDetailModel.FirstName,
                        LastName = patronDetailModel.LastName,
                        Address = patronDetailModel.Address,
                        DateOfBirth = patronDetailModel.DateOfBirth,
                        TelePhoneNumber = patronDetailModel.TelePhoneNumber,
                        HomeLibraryBranchId = patronDetailModel.HomeLibraryBranchId,
                        LibraryCardId = patronLibrarycard.Id
                    };
                    _context.Add(patron);
                    _context.SaveChanges();
                    return RedirectToAction("PatronList");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Patron is already exist";
            }
            
            return View();
        }
        public IActionResult PatronList()
        {
            var allPatrons = _patron.GetAll();
            var patronModels = allPatrons.Select(p => new PatronDetailModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                LibraryCardId = p.LibraryCard.Id,
                OverduFees = p.LibraryCard.Fees,
                HomeLibraryBranch = p.HomeLibraryBranch.Name
            }).ToList();


            var model = new PatronIndexModel()
            {
                Patrons = patronModels
            };
            return View(model);
        }
        public IActionResult Detail(int id)
        {
            var patron = _patron.Get(id);
            var model = new PatronDetailModel
            {
                FirstName = patron.FirstName,
                LastName = patron.LastName,
                Address = patron.Address,
                HomeLibraryBranch = patron.HomeLibraryBranch.Name,
                MemberSince = patron.LibraryCard.Created,
                OverduFees = patron.LibraryCard.Fees,
                LibraryCardId = patron.LibraryCard.Id,
                TelePhoneNumber = patron.TelePhoneNumber,
                AssetsCheckedOut = _patron.GetCheckOuts(id).ToList() ?? new List<CheckOut>(),
                CheckOutHistroy = _patron.GetCheckoutHistories(id),
                Holds = _patron.GetHolds(id)

            };
            return View(model);
        }
    }
}
