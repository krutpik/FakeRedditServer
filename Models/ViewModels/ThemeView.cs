using System.ComponentModel.DataAnnotations;

namespace FakeReddit.Models;

public class ThemeView
{
    public int Id { get; set; }
    [Required]
    [Length(minimumLength: 1, maximumLength: 32)]
    public string? Title { get; set; }
    [Required]
    [Length(minimumLength: 1, maximumLength: 40000)]
    public string? Content { get; set; }
    public int views { get; set; }
    public int Rate { get; set; }
    public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
    public string? Writer { get; set; }
    public List<CommentView>? Comments { get; set; }
}