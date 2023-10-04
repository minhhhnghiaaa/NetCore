using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NetCore.DataServices.Repositories.Interfaces;

namespace NetCore.Test.Controller;

public class BaseControllerTest : ControllerBase
{
    protected readonly Mock<IMapper> _mapper;
    protected readonly Mock<IConfiguration> _configuration;
    protected readonly Mock<IUnitOfWork> _unitOfWork;

    public BaseControllerTest(Mock<IMapper> mapper, Mock<IConfiguration> configuration, Mock<IUnitOfWork> unitOfWork)
    {
        _mapper = mapper;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }
    
}