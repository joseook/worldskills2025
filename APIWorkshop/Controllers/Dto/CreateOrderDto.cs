using APIWorkshop.Model;

namespace APIWorkshop.Controllers.Dto {
    public class CreateOrderDto {
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public string Channel { get; set; }
        public int StoreId { get; set; }
        public string PromotionId { get; set; }
        public decimal DiscountAmount { get; set; }
        public Customer Customer { get; set; }
    }
}
