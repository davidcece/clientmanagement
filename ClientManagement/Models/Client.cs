using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string? InternationalPhone { get; set; }
        public string EmailStatus { get; set; }
        public string SmsStatus { get; set; }
    }
}
