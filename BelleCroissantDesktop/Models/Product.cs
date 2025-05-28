using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BelleCroissantDesktop.Models
{
    public class Product
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(100)]
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }
        
        [Required]
        [JsonPropertyName("category")]
        public string Category { get; set; }
        
        [JsonPropertyName("ingredients")]
        public string Ingredients { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("seasonal")]
        public bool Seasonal { get; set; }
        
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        
        [Required]
        [JsonPropertyName("introducedDate")]
        public DateTime IntroducedDate { get; set; }
    }

    public class CreateProductDto
    {
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        public string Ingredients { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
        
        public string Description { get; set; }
        
        public bool Seasonal { get; set; }
        
        public bool Active { get; set; }
        
        [Required]
        public DateTime IntroducedDate { get; set; }
    }

    public class UpdateProductDto
    {
        public string ProductName { get; set; }
        
        public string Category { get; set; }
        
        public string Ingredients { get; set; }
        
        public decimal? Price { get; set; }
        
        public decimal? Cost { get; set; }
        
        public string Description { get; set; }
        
        public bool? Seasonal { get; set; }
        
        public bool? Active { get; set; }
    }
}
