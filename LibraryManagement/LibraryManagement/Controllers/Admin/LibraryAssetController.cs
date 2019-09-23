using LibraryData;
using LibraryData.Models;
using LibraryManagement.Models.LibraryAsset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers.Admin
{
    public class LibraryAssetController : Controller
    {
        private readonly LibraryContext _context;

        public LibraryAssetController(LibraryContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.Books);
        }
       
        public IActionResult Create()
        {
            LibraryAssetCV libraryAssetCV = new LibraryAssetCV();
            libraryAssetCV.StatusesList = _context.Statuses.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            libraryAssetCV.LibraryBranchesList = _context.LibraryBranches.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            return View(libraryAssetCV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LibraryAssetCV libraryAssetCV)
        {
            if (ModelState.IsValid)
            {
                var duplicate = _context.Books.Any(a => a.Author == libraryAssetCV.Author && a.ISBN == libraryAssetCV.ISBN);
                if (duplicate)
                {
                    Book book = new Book()
                    {
                        NumberOfCopies = libraryAssetCV.NumberOfCopies,
                        DewyIndex = libraryAssetCV.DewyIndex,
                        Author = libraryAssetCV.Author,
                        Cost = libraryAssetCV.Cost,
                        ImageUrl = libraryAssetCV.ImageUrl,
                        ISBN = libraryAssetCV.ISBN,
                        Location = libraryAssetCV.Location,
                        Title = libraryAssetCV.Title,
                        Year = libraryAssetCV.Year,


                    };
                    _context.Add(book);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Item is already exist";
                }
                
                
            }
            return View(libraryAssetCV);
        }

        // GET: Book/Edit/5
        public IActionResult Edit(int id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var book = _context.Books.Include(a => a.Location).Include(a => a.status).Where(c=>c.Id==id).FirstOrDefault();
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                LibraryAssetCV libraryAssetCV = new LibraryAssetCV()
                {
                    NumberOfCopies = book.NumberOfCopies,
                    DewyIndex = book.DewyIndex,
                    Author = book.Author,
                    Cost = book.Cost,
                    ImageUrl = book.ImageUrl,
                    ISBN = book.ISBN,
                    Location = book.Location,
                    Title = book.Title,
                    Year = book.Year,


                };
                
                ViewBag.LocationList = new SelectList(_context.LibraryBranches.ToList(), "Id", "Name", book.Location.Id);
                ViewBag.StatusList = new SelectList(_context.Statuses.ToList(), "Id", "Name", book.status.Id);
                return View(libraryAssetCV);
            }

        }
        [HttpPost]
        public IActionResult Edit(LibraryAsset libraryAsset)
        {
            _context.Entry(libraryAsset).State = EntityState.Modified;

             var isSaved=_context.SaveChanges() > 0;
            if (isSaved)
            {
                return RedirectToAction("Index");
            }
            return View(libraryAsset);
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.Books
                .FirstOrDefault(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.Books
                .FirstOrDefault(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll(int id)
        {
            var book = _context.Books.Find(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
