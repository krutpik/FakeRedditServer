namespace FakeReddit.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    
    public string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    
    public int ThemeId { get; set; }
    public Theme Theme { get; set; } = null!;
    
}