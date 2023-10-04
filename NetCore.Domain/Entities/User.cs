namespace NetCore.Domain.Entities;

public class User: BaseEntity
{
    public string Username { get; set; } = String.Empty;
    public string PasswordHash { get; set; } = String.Empty;
}