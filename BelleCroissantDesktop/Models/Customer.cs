using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BelleCroissantDesktop.Models
{
    public class Customer
    {
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        // Propriedade calculada para o nome completo
        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";
    }
}
