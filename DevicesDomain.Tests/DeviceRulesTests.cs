using DevicesDomain.DTOs;
using DevicesDomain.Models;
using DevicesDomain.ValidationRules;
using FluentAssertions;
using Xunit;

namespace DevicesDomain.Tests;

public class DeviceRulesTests
{
    [Fact]
    public void ValidateUpdate_ShouldThrow_WhenNameChangesWhileDeviceIsInUse()
    {
        var device = new Device
        {
            Name = "Original",
            Brand = "Samsung",
            State = DeviceState.InUse
        };

        var dto = new DeviceUpdateDto
        {
            Name = "Novo Nome"
        };

        Action act = () => DeviceRules.ValidateUpdate(device, dto);
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Name cannot be updated while device is in use.");
    }

    [Fact]
    public void ValidateUpdate_ShouldThrow_WhenBrandChangesWhileDeviceIsInUse()
    {
        var device = new Device
        {
            Name = "Device",
            Brand = "Samsung",
            State = DeviceState.InUse
        };

        var dto = new DeviceUpdateDto
        {
            Brand = "LG"
        };

        Action act = () => DeviceRules.ValidateUpdate(device, dto);
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Brand cannot be updated while device is in use.");
    }

    [Fact]
    public void ValidateUpdate_ShouldNotThrow_WhenOnlyStateChangesWhileDeviceIsAvaliable()
    {
        var device = new Device
        {
            Name = "Device",
            Brand = "Samsung",
            State = DeviceState.Available
        };

        var dto = new DeviceUpdateDto
        {
            State = "InUse"
        };

        Action act = () => DeviceRules.ValidateUpdate(device, dto);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateUpdate_ShouldNotThrow_WhenOnlyStateChangesWhileDeviceIsInactive()
    {
        var device = new Device
        {
            Name = "Device",
            Brand = "Samsung",
            State = DeviceState.Inactive
        };

        var dto = new DeviceUpdateDto
        {
            State = "Available"
        };

        Action act = () => DeviceRules.ValidateUpdate(device, dto);
        act.Should().NotThrow();
    }
}
