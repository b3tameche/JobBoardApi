using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace update.Models.Domain;

public class User : IdentityUser
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Company { get; set; }
    [Required]
    public ICollection<Job> Jobs { get; set; }
}