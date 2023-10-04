using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NetCore.Api.Controllers;
using NetCore.Api.Dtos.Requests.Merchant;
using NetCore.Api.Dtos.Responses.Merchant;
using NetCore.Api.MappingProfiles.Merchant;
using NetCore.DataServices.Repositories.Interfaces;
using NetCore.Domain.Entities;

namespace NetCore.Test.Controller;

public class MemberControllerTests
{
    private readonly MerchantController _controller;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IConfiguration> _configuration;
    private readonly Fixture _fixture;
    private readonly Mock<IMapper> _mockMapper;

    public MemberControllerTests()
    {
        // fixture for creating test data
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        // mock user repo dependency
        _mockUnitOfWork = new Mock<IUnitOfWork>();


        // automapper dependency

        var mapper = new MapperConfiguration(x =>
            {
                x.AddProfile<DomainToResponse>();
                x.AddProfile<RequestToDomain>();
            }
        ).CreateMapper();

        var configurationMock = new Mock<IConfiguration>();

        var configuration = configurationMock.Object;
        _mockMapper = new Mock<IMapper>();

        // service under test
        _controller = new MerchantController(_mockUnitOfWork.Object, mapper, configuration);
    }

    [Fact]
    public async Task GetAll_Return200()
    {
        //Arrange
        const int expectedCount = 2;
        var expectedMerchants = _fixture.CreateMany<Merchant>(expectedCount);
        var enumerable = expectedMerchants as Merchant[] ?? expectedMerchants.ToArray();
        _mockUnitOfWork.Setup(x => x.Merchants.All()).ReturnsAsync(enumerable);

        //Act
        var merchant = await _controller.GetAllMerchant();
        // var merchantList = (merchant as OkObjectResult)?.Value as IEnumerable<Merchant>;

        // var pageResponse = result!.Value as PageResponse<IEnumerable<MemberDisplayDto>>;

        //Assert
        Assert.NotNull(merchant);
        Assert.IsType<OkObjectResult>(merchant);

        var okResult = Assert.IsType<OkObjectResult>(merchant);
        var returnValue = Assert.IsType<List<GetMerchantResponseDtos>>(okResult.Value);
        Assert.Equal(enumerable.FirstOrDefault()!.Balance, returnValue.FirstOrDefault()!.Balance);
    }

    [Fact]
    public async Task GetById_Return200()
    {
        //Arrange
        var expectedMerchant = _fixture.Create<Merchant>();
        _mockUnitOfWork.Setup(x => x.Merchants.GetById(expectedMerchant.Id)).ReturnsAsync(expectedMerchant);

        //Act
        var merchant = await _controller.GetMerchant(expectedMerchant.Id);
        // var merchantList = (merchant as OkObjectResult)?.Value as IEnumerable<Merchant>;

        //Assert
        Assert.NotNull(merchant);
        Assert.IsType<OkObjectResult>(merchant);

        var okResult = Assert.IsType<OkObjectResult>(merchant);
        var returnValue = Assert.IsType<GetMerchantResponseDtos>(okResult.Value);
        Assert.Equal(expectedMerchant.Balance, returnValue.Balance);
        Assert.Equal(expectedMerchant.Id, returnValue.MerchantId);
        Assert.Equal(expectedMerchant.BankName, returnValue.BankName);
        Assert.Equal(expectedMerchant.AccountNumber, returnValue.AccountNumber);
        Assert.Equal(expectedMerchant.SwiftCode, returnValue.SwiftCode);
    }

    [Fact]
    public async Task GetById_Return404()
    {
        //Arrange
        var expectedMerchant = _fixture.Create<Merchant>();
        _mockUnitOfWork.Setup(x => x.Merchants.GetById(expectedMerchant.Id)).ReturnsAsync(expectedMerchant);

        //Act
        var merchant = await _controller.GetMerchant(Guid.NewGuid());

        //Assert
        Assert.NotNull(merchant);
        Assert.IsType<NotFoundResult>(merchant);

        var notFoundResult = Assert.IsType<NotFoundResult>(merchant);
        Assert.Equal(404, notFoundResult.StatusCode);
        // Assert.Equal("Merchant not found", notFoundResult.Value);
    }

    [Fact]
    public async Task Add_Return201()
    {
        //Arrange
        _mockMapper.Setup(x => x.Map<CreateMerchantRequestDtos>(It.IsAny<Merchant>()))
            .Returns((Merchant source) => new CreateMerchantRequestDtos
            {
                SwiftCode = source.SwiftCode,
                Name = source.Name,
                BankName = source.BankName,
                AccountNumber = source.AccountNumber,
                Balance = source.Balance
            });
        var merchant1 = _fixture.Create<Merchant>();
        var merchant = _mockMapper.Object.Map<CreateMerchantRequestDtos>(merchant1);
        _mockUnitOfWork.Setup(x => x.Merchants.Add(merchant1)).ReturnsAsync(true);
        
        //Act
        var merchantResponse = await _controller.AddMerchant(merchant);

        //Assert
        Assert.NotNull(merchantResponse);

        var createdResult = Assert.IsType<CreatedAtActionResult>(merchantResponse);
        var returnValue = Assert.IsType<Merchant>(createdResult.Value);
        Assert.Equal(merchant1.SwiftCode, returnValue.SwiftCode);
        Assert.Equal(merchant1.Name, returnValue.Name);
        Assert.Equal(merchant1.BankName, returnValue.BankName);
        Assert.Equal(merchant1.AccountNumber, returnValue.AccountNumber);
        Assert.Equal(201, createdResult.StatusCode);
    }
}