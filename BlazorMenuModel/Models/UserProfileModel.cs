using System.ComponentModel.DataAnnotations;

namespace BlazorMenuModel.Models
{
    public class UserProfileModel
    {
        [Required]
        public string CUSER_ID { get; set; }

        [Required(ErrorMessage = "The Username field is required.")]
        [StringLength(30, MinimumLength = 5)]
        public string CUSER_NAME { get; set; }

        [Required(ErrorMessage = "The Email Address field is required.")]
        [EmailAddress]
        public string CEMAIL_ADDRESS { get; set; }

        public string CLAST_UPDATE_PASSWORD { get; set; }
        public string CPOSITION { get; set; }

        [Required]
        public string CCULTURE_ID { get; set; }
    }
}
