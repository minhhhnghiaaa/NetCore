using System.ComponentModel.DataAnnotations;

namespace NetCore.Api.Dtos.Requests.User;

public class CreateUserRequestDtos
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}