using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MinimalChatApplicationAPI.Models
{
    public class User : IdentityUser<string>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)] // Adjust the max length as needed
        public string Email { get; set; }

        [Required]
        [MaxLength(255)] // Adjust the max length as needed
        public string Password{ get; set; }
    }
}
