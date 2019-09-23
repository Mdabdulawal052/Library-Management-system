using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models.LibraryBranchNew
{
    public class LibraryBranchCv
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Limit branch name to 30 Characters.")]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Telephone { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public string ImageUrl { get; set; }
        public virtual List<BranchHours> BranchHourList { get; set; }
        [Range(0, 6)]
        public int DayOfWeek { get; set; }
        [Range(0, 23)]
        public int OpenTime { get; set; }
        [Range(0, 23)]
        public int CloseTime { get; set; }
    }
}
