using System.ComponentModel.DataAnnotations;

namespace ModalLayer.Modal
{
    public class RequestModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(15)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [EmailAddress, MaxLength(255)]
        public string? Email { get; set; }

        public string? Address { get; set; }
    }
}
