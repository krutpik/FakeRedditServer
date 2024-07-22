namespace FakeReddit.Models;

public struct User(string email, string password, string role)
{
    public readonly string Email = email;
    public readonly string Password = password; 
    public readonly string Role = role;
    
}