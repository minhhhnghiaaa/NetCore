namespace NetCore.Api.Dtos.Requests.Merchant;

public class CreateMerchantRequestDtos
{
    public string Name { get; set; } = string.Empty;
    public string BankName { get; set; } 
    public string AccountNumber { get; set; } 
    public string SwiftCode { get; set; } 
    public int Balance { get; set; }
}