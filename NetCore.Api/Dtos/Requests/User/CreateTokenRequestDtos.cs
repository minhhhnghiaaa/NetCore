using System.ComponentModel.DataAnnotations;

namespace NetCore.Api.Dtos.Requests.User;

public class CreateTokenRequestDtos
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}