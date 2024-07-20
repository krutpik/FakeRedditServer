using Microsoft.EntityFrameworkCore;

namespace FakeReddit.Models;

public class Rate
{
    public int Id { get; set; }
    public bool IsRate { get; set; }
    
    public string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    
    public int ThemeId { get; set; }
    public Theme Theme { get; set; } = null!;
}