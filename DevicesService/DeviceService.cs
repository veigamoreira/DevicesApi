using AutoMapper;
using DevicesDomain.DTOs;
using DevicesDomain.Models;
using DevicesRepository.Interfaces;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _repository;
    private readonly IMapper _mapper;

    public DeviceService(IDeviceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<DeviceReadDto>> GetAllAsync()
    {
        var devices = await _repository.GetAllAsync();
        return _mapper.Map<List<DeviceReadDto>>(devices);
    }

    public async Task<DeviceReadDto?> GetByIdAsync(Guid id)
    {
        var device = await _repository.GetByIdAsync(id);
        return device == null ? null : _mapper.Map<DeviceReadDto>(device);
    }

    public async Task<List<DeviceReadDto>> GetByBrandAsync(string brand)
    {
        var devices = await _repository.GetByBrandAsync(brand);
        return _mapper.Map<List<DeviceReadDto>>(devices);
    }

    public async Task<List<DeviceReadDto>> GetByStateAsync(DeviceState state)
    {
        var devices = await _repository.GetByStateAsync(state);
        return _mapper.Map<List<DeviceReadDto>>(devices);
    }

    public async Task<DeviceReadDto> CreateAsync(DeviceCreateDto dto)
    {
        var device = _mapper.Map<Device>(dto);
        device.Id = Guid.NewGuid();
        device.CreatedAt = DateTime.UtcNow;

        var created = await _repository.CreateAsync(device);
        return _mapper.Map<DeviceReadDto>(created);
    }

    public async Task<DeviceReadDto?> UpdateAsync(Guid id, DeviceUpdateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        // Regra: não permitir alteração de Name ou Brand se o dispositivo estiver em uso
        if (existing.State == DeviceState.InUse)
        {
            if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != existing.Name)
                throw new InvalidOperationException("Name cannot be updated while device is In-use.");

            if (!string.IsNullOrWhiteSpace(dto.Brand) && dto.Brand != existing.Brand)
                throw new InvalidOperationException("Brand cannot be updated while device is In-use.");
        }


        _mapper.Map(dto, existing);
        var updated = await _repository.UpdateAsync(existing);
        return updated ? _mapper.Map<DeviceReadDto>(existing) : null;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var device = await _repository.GetByIdAsync(id);
        if (device == null) return false;

        // Regra: não permitir deletar se o dispositivo estiver em uso

        if (device.State == DeviceState.InUse)
            throw new InvalidOperationException("In-use devices cannot be deleted.");

        return await _repository.DeleteAsync(id);
    }
}
