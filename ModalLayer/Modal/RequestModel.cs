using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModalLayer.Modal
{
    public class RequestModel
    {
            [Required, MaxLength(100)]
            public string Name { get; set; }

            [Required, Phone, MaxLength(15)]
            public string PhoneNumber { get; set; }

            [EmailAddress, MaxLength(255)]
            public string? Email { get; set; }

            public string? Address { get; set; }
        }

    }

