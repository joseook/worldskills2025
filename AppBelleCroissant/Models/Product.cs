using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBelleCroissant.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Ingredients { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public bool Seasonal { get; set; }
        public bool Active { get; set; }
        public DateTime IntroducedDate { get; set; }
        public bool IsFavorite { get; set; } // Para controle local de favoritos
    }
}
