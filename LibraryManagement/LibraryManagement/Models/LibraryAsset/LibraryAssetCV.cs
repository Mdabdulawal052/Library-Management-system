
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models.LibraryAsset
{
    public class LibraryAssetCV
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public virtual Status Statuss { get; set; }
        [Required]
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string DewyIndex { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int StatusId { get; set; }
        public virtual LibraryBranch Location { get; set; }

        public IEnumerable<SelectListItem> LibraryBranchesList { get; set; }
        public IEnumerable<SelectListItem> StatusesList { get; set; }

    }
}
