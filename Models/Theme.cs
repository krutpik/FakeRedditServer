using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FakeReddit.Models;

public class Theme
{
    public int Id { get; set; }
    [Required]
    [Length(minimumLength: 1, maximumLength: 32)]
    public string? Title { get; set; }

    public int Rate { get; set; } = 0;
    public DateTime Date { get; set; }
    public string? Writer { get; set; } = "noname";
}