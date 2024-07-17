namespace FakeReddit.Models;

public class ThemeView
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int Rate { get; set; } = 0;
    public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
    public string? Writer { get; set; }

}