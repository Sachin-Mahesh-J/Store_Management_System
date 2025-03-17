using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Managment_WPF_UI.Services
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public int? CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int MinStockLevel { get; set; }

        // Display properties
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
    }
}
