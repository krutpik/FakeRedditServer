﻿using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace FakeReddit.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Theme> Themes { get; set; }
}

public class Theme
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    [Required]
    [Length(minimumLength: 1, maximumLength: 32)]
    public string? Title { get; set; }
    public int Rate { get; set; }
    public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
}

