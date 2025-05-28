namespace APIWorkshop.Controllers.Dto {
    public class CreateProductDto {
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Ingredients { get; set; }
        public bool Seasonal { get; set; }
        public bool Active { get; set; }

        public DateTime IntroducedDate { get; set; } = DateTime.Now;
    }
}
