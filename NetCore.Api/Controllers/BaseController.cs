using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.DataServices.Repositories.Interfaces;

namespace NetCore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BaseController : ControllerBase
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly IConfiguration _configuration;   

    public BaseController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }
}