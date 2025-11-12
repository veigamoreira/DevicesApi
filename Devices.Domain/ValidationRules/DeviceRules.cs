using DevicesDomain.DTOs;
using DevicesDomain.Models;

namespace DevicesDomain.ValidationRules
{
    public static class DeviceRules
    {
        public static void ValidateUpdate(Device device, DeviceUpdateDto dto)
        {
            if (device.State == DeviceState.InUse)
            {
                if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != device.Name)
                    throw new BusinessRuleException("Name cannot be updated while device is in use.");

                if (!string.IsNullOrWhiteSpace(dto.Brand) && dto.Brand != device.Brand)
                    throw new BusinessRuleException("Brand cannot be updated while device is in use.");
            }
        }

        public static void ValidateDelete(Device device)
        {
            if (device.State == DeviceState.InUse)
                throw new BusinessRuleException("In-use devices cannot be deleted.");
        }
    }
}
