using System.ComponentModel.DataAnnotations;

namespace DrugFinderMVC.Models
{
    // ── Account ──────────────────────────────────────────────
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    // ── Patient ───────────────────────────────────────────────
    public class RegisterPatientViewModel
    {
        [Required, StringLength(30, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FName { get; set; } = string.Empty;

        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string SName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords don't match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required, Range(-90, 90)]
        public double Latitude { get; set; }

        [Required, Range(-180, 180)]
        public double Longitude { get; set; }
    }

    // ── Pharmacy ──────────────────────────────────────────────
    public class RegisterPharmacyViewModel
    {
        [Required, StringLength(30, MinimumLength = 3)]
        [Display(Name = "Pharmacy Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3)]
        public string? Address { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords don't match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Licence Number")]
        public string LiscenceNumber { get; set; } = string.Empty;

        [Required, Range(-90, 90)]
        public double Latitude { get; set; }

        [Required, Range(-180, 180)]
        public double Longitude { get; set; }
    }

    // ── Drug Request ──────────────────────────────────────────
    public class DrugDetailViewModel
    {
        [Required(ErrorMessage = "Drug name is required")]
        [Display(Name = "Drug Name")]
        public string DrugName { get; set; } = string.Empty;

        [Display(Name = "Strength")]
        public string Strength { get; set; } = string.Empty;

        [Required, Range(1, 6, ErrorMessage = "Please select a valid form")]
        public int Form { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }

    public class CreateDrugRequestViewModel
    {
        public List<DrugDetailViewModel> Drugs { get; set; } = new() { new DrugDetailViewModel() };
    }

    // ── API Response wrapper ──────────────────────────────────
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}
