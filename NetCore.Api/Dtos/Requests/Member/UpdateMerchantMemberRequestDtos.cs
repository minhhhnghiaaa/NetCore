namespace NetCore.Api.Dtos.Requests.Member;

public class UpdateMerchantMemberRequestDtos
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Guid MerchantId { get; set; }
    public int Status { get; set; }
}