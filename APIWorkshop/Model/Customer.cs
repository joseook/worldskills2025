namespace APIWorkshop.Model {
    public class Customer {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int Frequency { get; set; }
        public string PreferredCategory { get; set; }
        public bool Churned { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
