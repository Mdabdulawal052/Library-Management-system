using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models.Catalog
{
    public class AssetIndexModel
    {
        public IEnumerable<AssetIndexListingModels> Assets { get; set; }
    }
}
