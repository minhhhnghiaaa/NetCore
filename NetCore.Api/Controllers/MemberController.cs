using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCore.Api.Dtos.Requests.Member;
using NetCore.Api.Dtos.Responses.Member;
using NetCore.DataServices.Repositories.Interfaces;
using NetCore.Domain.Entities;


namespace NetCore.Api.Controllers;

public class MemberController : BaseController
{
    public MemberController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork,
        mapper, configuration)
    {
    }

    [HttpGet]
    [Route("{memberId:guid}")]
    public async Task<IActionResult> GetMember(Guid memberId)
    {
        var member = await _unitOfWork.Members.GetById(memberId);

        if (member == null)
            return NotFound();

        var result = _mapper.Map<MerchantMemberResponseDtos>(member);
        return Ok(result);
    }

    [HttpGet]
    [Route("merchant/{merchantId:guid}")]
    public async Task<IActionResult> GetMerchantMember(Guid merchantId)
    {
        var merchantMember = await _unitOfWork.Members.GetMemberMerchantAsync(merchantId);
        var result = _mapper.Map<IEnumerable<MerchantMemberResponseDtos>>(merchantMember);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddMember([FromBody] CreateMerchantMemberRequestDtos member)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _mapper.Map<Member>(member);
        var checkExits = await _unitOfWork.Members.GetByEmail(result.Email);
        if (checkExits)
        {
            return Conflict();
        }

        await _unitOfWork.Members.Add(result);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetMerchantMember), new { merchantId = result.MerchantId }, result);
    }

    [HttpPut]
    // [Route("{memberId:guid}")]
    public async Task<IActionResult> UpdateMember([FromBody] UpdateMerchantMemberRequestDtos member)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var memberDb = await _unitOfWork.Members.GetById(member.Id);
        var memberUpdate = _mapper.Map<Member>(member);
        if (memberDb != null) memberUpdate.CreatedDate = memberDb.CreatedDate;

        var checkExits = await _unitOfWork.Members.GetByEmail(member.Email);
        if (checkExits)
        {
            return Conflict();
        }

        await _unitOfWork.Members.Update(memberUpdate);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetMember), new { memberId = member.Id }, member);
    }

    [HttpDelete]
    [Route("{memberId:guid}")]
    public async Task<IActionResult> DeleteMember(Guid memberId)
    {
        var member = await _unitOfWork.Members.GetById(memberId);
        if (member == null)
            return NotFound("Member not found");

        await _unitOfWork.Members.Delete(memberId);
        await _unitOfWork.CompleteAsync();

        // var result = await _unitOfWork.Members.All();
        var result = await _unitOfWork.Members.GetById(memberId);
        return Ok(result);
    }
}