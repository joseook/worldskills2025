using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BelleCroissantDesktop.Models
{
    public class Order
    {
        [JsonPropertyName("transactionId")]
        public int TransactionId { get; set; }

        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("product")]
        public Product Product { get; set; }

   
        [JsonIgnore]
        public decimal TotalAmount => Quantity * Price;
    }

    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public string Channel { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }
}
