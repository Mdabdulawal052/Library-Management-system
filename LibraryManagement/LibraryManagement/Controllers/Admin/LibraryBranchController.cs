using LibraryData;
using LibraryData.Models;
using LibraryManagement.Models.LibraryBranchNew;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers.Admin
{
    public class LibraryBranchController : Controller
    {
        private readonly LibraryContext _context;

        public LibraryBranchController(LibraryContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.LibraryBranches);
        }

        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(LibraryBranchCv libraryBranchCv)
        {
            if (ModelState.IsValid)
            {
                var duplicate = _context.LibraryBranches.Any(a => a.Name == libraryBranchCv.Name && a.Telephone == libraryBranchCv.Telephone);
                if (duplicate)
                {
                    LibraryBranch libraryBranch = new LibraryBranch()
                    {
                        Name = libraryBranchCv.Name,
                        Address = libraryBranchCv.Address,
                        Description = libraryBranchCv.Description,
                        ImageUrl = "/images/" + libraryBranchCv.ImageUrl,
                        Telephone = libraryBranchCv.Telephone,
                        OpenDate = libraryBranchCv.OpenDate,
                        BranchHourList = libraryBranchCv.BranchHourList
                    };
                    _context.Add(libraryBranch);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Item is already exist";
                }
                
               
            }
            return View(libraryBranchCv);
    }

    //public IActionResult Edit(int id)
    //    {
    //        //if (id == null)
    //        //{
    //        //    return NotFound();
    //        //}

    //        var branch = _context.LibraryBranches.Include(a=>a.BranchHourList).Where(c => c.Id == id).FirstOrDefault();
    //        if (branch == null)
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
                
    //            return View(branch);
    //        }

    //    }
    //    [HttpPost]
    //    public IActionResult Edit(LibraryBranch libraryBranch)
    //    {
    //        _context.Entry(libraryBranch).State = EntityState.Modified;

    //        var isSaved = _context.SaveChanges() > 0;
    //        if (isSaved)
    //        {
    //            return RedirectToAction("Index");
    //        }
    //        return View(libraryBranch);
    //    }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryBranch = _context.LibraryBranches
                .FirstOrDefault(m => m.Id == id);
            if (libraryBranch == null)
            {
                return NotFound();
            }

            return View(libraryBranch);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.LibraryBranches.Include(b=>b.BranchHourList)
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
            var branches = _context.LibraryBranches.Include(b => b.BranchHourList)
                .FirstOrDefault(m => m.Id == id);
            if(branches == null)
            {
                return NotFound();
            }
            _context.LibraryBranches.Remove(branches);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}