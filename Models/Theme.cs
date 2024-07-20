using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace FakeReddit.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Theme> Themes { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Rate> Rates { get; set; }
}

public class Theme
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    [Required]
    [Length(minimumLength: 1, maximumLength: 32)]
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
    public int views { get; set; }
    public virtual List<Comment> Comments { get; set; }
    public virtual ICollection<Rate> Rates { get; set; }
}

