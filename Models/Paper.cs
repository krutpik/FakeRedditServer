using System.ComponentModel.DataAnnotations;

namespace FakeReddit.Models;

public class Paper
{
    public int Id { get; set; }
    [Required]
    [Length(minimumLength: 1, maximumLength: 32)]
    public string? Title { get; set; }
    [Required]
    [Length(minimumLength: 1, maximumLength: 1200)]
    public string? Content { get; set; }
    [Required]
    public DateTime Data { get; set; }
    [Required]
    public string? Writer { get; set; } = "noname";
}