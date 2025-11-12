using AutoMapper;
using DevicesDomain.DTOs;
using DevicesDomain.Models;
using DevicesDomain.ValidationRules;
using DevicesRepository.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace DevicesDomain.Tests;
public class DeviceServiceTests
{
    private readonly Mock<IDeviceRepository> _repoMock = new();
    private readonly IMapper _mapper;

    public DeviceServiceTests()
    {
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Device, DeviceReadDto>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()));

            cfg.CreateMap<DeviceCreateDto, Device>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => ParseState(src.State)));

            cfg.CreateMap<DeviceUpdateDto, Device>()
                          .ForMember(dest => dest.State, opt => opt.MapFrom(src => ParseState(src.State)))
                          .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        });
        _mapper = config.CreateMapper();
    }


    private static DeviceState ParseState(string state)
    {
        return Enum.TryParse<DeviceState>(state, true, out var parsed)
            ? parsed
            : DeviceState.Inactive;
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnReadDto_WhenValid()
    {
        var dto = new DeviceCreateDto
        {
            Name = "Sensor",
            Brand = "Acme",
            State = "Available"
        };

        var created = new Device
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Brand = dto.Brand,
            State = DeviceState.Available,
            CreatedAt = DateTime.UtcNow
        };

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<Device>())).ReturnsAsync(created);

        var service = new DeviceService(_repoMock.Object, _mapper);
        var result = await service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Name.Should().Be("Sensor");
        result.State.Should().Be("Available");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenDeviceIsInUseAndNameChanges()
    {
        var id = Guid.NewGuid();
        var existing = new Device
        {
            Id = id,
            Name = "Sensor",
            Brand = "Acme",
            State = DeviceState.InUse
        };

        var dto = new DeviceUpdateDto
        {
            Name = "Novo Nome"
        };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

        var service = new DeviceService(_repoMock.Object, _mapper);

        Func<Task> act = async () => await service.UpdateAsync(id, dto);
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Name cannot be updated while device is in use.");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenDeviceIsInUseAndBrandChanges()
    {
        var id = Guid.NewGuid();
        var existing = new Device
        {
            Id = id,
            Name = "Sensor",
            Brand = "Acme",
            State = DeviceState.InUse
        };

        var dto = new DeviceUpdateDto
        {
            Brand = "Novo brand"
        };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

        var service = new DeviceService(_repoMock.Object, _mapper);

        Func<Task> act = async () => await service.UpdateAsync(id, dto);
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Brand cannot be updated while device is in use.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenDeviceInUse()
    {
        var id = Guid.NewGuid();
        var device = new Device
        {
            Id = id,
            State = DeviceState.InUse
        };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(device);

        var service = new DeviceService(_repoMock.Object, _mapper);

        Func<Task> act = async () => await service.DeleteAsync(id);
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("In-use devices cannot be deleted.");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDto_WhenDeviceExists()
    {
        var id = Guid.NewGuid();
        var device = new Device
        {
            Id = id,
            Name = "Sensor",
            Brand = "Acme",
            State = DeviceState.Available,
            CreatedAt = DateTime.UtcNow
        };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(device);

        var service = new DeviceService(_repoMock.Object, _mapper);
        var result = await service.GetByIdAsync(id);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be("Sensor");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenDeviceIsAvailable()
    {
        var id = Guid.NewGuid();
        var device = new Device
        {
            Id = id,
            State = DeviceState.Available
        };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(device);
        _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new DeviceService(_repoMock.Object, _mapper);
        var result = await service.DeleteAsync(id);

        result.Should().BeTrue();
    }
}
