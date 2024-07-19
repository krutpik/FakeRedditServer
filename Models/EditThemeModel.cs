using System.ComponentModel.DataAnnotations;

namespace FakeReddit.Models;

public class EditThemeModel
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    [Required]
    public string? Content { get; set; }

}