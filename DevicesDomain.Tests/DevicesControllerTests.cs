using DevicesApi.Controllers;
using DevicesDomain.DTOs;
using DevicesDomain.ValidationRules;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DevicesDomain.Tests;

public class DevicesControllerTests
{
    private readonly Mock<IDeviceService> _serviceMock = new();
    private readonly DevicesController _controller;

    public DevicesControllerTests()
    {
        _controller = new DevicesController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithList()
    {
        var devices = new List<DeviceReadDto> {
            new() { Id = Guid.NewGuid(), Name = "Sensor", Brand = "Acme", State = "Avaliable" }
        };

        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(devices);

        var result = await _controller.GetAll();
        var ok = result.Result as OkObjectResult;

        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(devices);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenFound()
    {
        var id = Guid.NewGuid();
        var dto = new DeviceReadDto { Id = id, Name = "Sensor", Brand = "Acme", State = "Avaliable" };

        _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(dto);

        var result = await _controller.GetById(id);
        var ok = result.Result as OkObjectResult;

        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenMissing()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((DeviceReadDto?)null);

        var result = await _controller.GetById(id);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenValid()
    {
        var dto = new DeviceCreateDto { Name = "Sensor", Brand = "Acme", State = "Inactive" };
        var created = new DeviceReadDto { Id = Guid.NewGuid(), Name = "Sensor", Brand = "Acme", State = "Inactive" };

        _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        var result = await _controller.Create(dto);
        var createdResult = result.Result as CreatedAtActionResult;

        createdResult.Should().NotBeNull();
        createdResult!.Value.Should().BeEquivalentTo(created);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenValid()
    {
        var id = Guid.NewGuid();
        var dto = new DeviceUpdateDto { State = "Standby" };
        var updated = new DeviceReadDto { Id = id, Name = "Sensor", Brand = "Acme", State = "Avaliable" };

        _serviceMock.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync(updated);

        var result = await _controller.Update(id, dto);
        var ok = result.Result as OkObjectResult;

        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(updated);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenBusinessRuleFails()
    {
        var id = Guid.NewGuid();
        var dto = new DeviceUpdateDto { Name = "Novo Nome" , State = "InUse" };

        _serviceMock.Setup(s => s.UpdateAsync(id, dto))
            .ThrowsAsync(new BusinessRuleException("Name cannot be updated while device is in use."));

        var result = await _controller.Update(id, dto);
        var badRequest = result.Result as BadRequestObjectResult;

        badRequest.Should().NotBeNull();
        badRequest!.Value.Should().BeEquivalentTo(new { error = "Name cannot be updated while device is in use." });
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var result = await _controller.Delete(id);
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenDeviceIsInUse()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.DeleteAsync(id))
            .ThrowsAsync(new BusinessRuleException("In-use devices cannot be deleted."));

        var result = await _controller.Delete(id);
        var badRequest = result as BadRequestObjectResult;

        badRequest.Should().NotBeNull();
        badRequest!.Value.Should().BeEquivalentTo(new { error = "In-use devices cannot be deleted." });
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenDeviceMissing()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        var result = await _controller.Delete(id);
        result.Should().BeOfType<NotFoundResult>();
    }
}
