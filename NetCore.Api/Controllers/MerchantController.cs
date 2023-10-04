using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCore.Api.Dtos.Requests.Merchant;
using NetCore.Api.Dtos.Responses.Merchant;
using NetCore.DataServices.Repositories.Interfaces;
using NetCore.Domain.Entities;


namespace NetCore.Api.Controllers;

public class MerchantController : BaseController
{
    public MerchantController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
    {
    }

    [HttpGet]
    [Route("{merchantId:guid}")]
    public async Task<IActionResult> GetMerchant(Guid merchantId)
    {
        var merchant = await _unitOfWork.Merchants.GetById(merchantId);

        if (merchant == null)
            return NotFound();

        var result = _mapper.Map<GetMerchantResponseDtos>(merchant);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddMerchant([FromBody] CreateMerchantRequestDtos merchant)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _mapper.Map<Merchant>(merchant);
        var checkExits = await _unitOfWork.Merchants.GetByCode(result.SwiftCode);
        if (checkExits)
        {
            return Conflict();
        }

        await _unitOfWork.Merchants.Add(result);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetMerchant), new { merchantId = result.Id }, result);
    }

    [HttpPut]
    // [Route("{merchantId:guid}")]
    public async Task<IActionResult> UpdateMerchant([FromBody] UpdateMerchantRequestDtos merchant)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var merchantDb =  await _unitOfWork.Merchants.GetById(merchant.Id);
        var merchantUpdate = _mapper.Map<Merchant>(merchant);
        if (merchantDb != null) merchantUpdate.CreatedDate = merchantDb.CreatedDate;

        var checkExits = await _unitOfWork.Merchants.GetByCode(merchant.SwiftCode);
        if (checkExits)
        {
            return Conflict();
        }
        
        await _unitOfWork.Merchants.Update(merchantUpdate);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetMerchant), new { merchantId = merchant.Id }, merchant);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMerchant()
    {
        var merchant = await _unitOfWork.Merchants.All();

        return Ok(_mapper.Map<IEnumerable<GetMerchantResponseDtos>>(merchant));
    }

    [HttpDelete]
    [Route("{merchantId:guid}")]
    public async Task<IActionResult> DeleteMerchant(Guid merchantId)
    {
        var merchant = await _unitOfWork.Merchants.GetById(merchantId);
        if (merchant == null)
            return NotFound("Merchant not found");

        await _unitOfWork.Merchants.Delete(merchantId);
        await _unitOfWork.CompleteAsync();

        // var result = await _unitOfWork.Merchants.All();

        return CreatedAtAction(nameof(GetMerchant), new { merchantId = merchant.Id }, merchant);
    }
    // [HttpDelete]
    // [Route("hard/{merchantId:guid}")]
    // public async Task<IActionResult> HardDeleteMerchant(Guid merchantId)
    // {
    //     var merchant = await _unitOfWork.Merchants.GetById(merchantId);
    //     if (merchant == null)
    //         return NotFound("Merchant not found");
    //
    //     await _unitOfWork.Merchants.Delete(merchantId);
    //     await _unitOfWork.CompleteAsync();
    //
    //     // var result = await _unitOfWork.Merchants.All();
    //
    //     return CreatedAtAction(nameof(GetMerchant), new { merchantId = merchant.Id }, merchant);
    // }
}