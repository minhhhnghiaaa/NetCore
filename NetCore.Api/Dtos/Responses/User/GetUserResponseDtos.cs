namespace NetCore.Api.Dtos.Responses.User;

public class GetUserResponseDtos
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public int Status { get; set; }
}