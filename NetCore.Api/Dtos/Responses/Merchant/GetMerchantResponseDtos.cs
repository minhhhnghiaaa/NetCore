namespace NetCore.Api.Dtos.Responses.Merchant;

public class GetMerchantResponseDtos
{
    public Guid MerchantId { get; set; }
    public string FullName { get; set; }
    public string BankName { get; set; }
    public string AccountNumber { get; set; }
    public string SwiftCode { get; set; }
    public int Balance { get; set; }
}