namespace NetCore.Api.Dtos.Requests.Merchant;

public class UpdateMerchantRequestDtos
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string BankName { get; set; } 
    public string AccountNumber { get; set; } 
    public string SwiftCode { get; set; } 
    public int Balance { get; set; }
    public int Status { get; set; }
}