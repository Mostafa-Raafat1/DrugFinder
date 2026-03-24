using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class RegisterPharmacyDTO
    {
        [Required]
        [StringLength(30, MinimumLength =3, ErrorMessage ="Name must be between 3 to 30 char")]
        public string Name { get; set; }
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Adress must be between 10 to 100 char")]
        public string? Address { get; set; }
        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Please enter  a valid email address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage ="Passwords Don't Match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string LiscenceNumber { get; set; }
        [Required]
        [Range(-90,90,ErrorMessage ="Invalid Latitude")]
        public double Latitude { get; set; }
        [Required]
        [Range(-180, 180, ErrorMessage = "Invalid Longitude")]
        public double Longitude { get; set; }

    }
}
