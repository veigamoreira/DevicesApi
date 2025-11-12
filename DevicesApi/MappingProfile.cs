using AutoMapper;
using DevicesDomain.DTOs;
using DevicesDomain.Models;

namespace DevicesApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Device, DeviceReadDto>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()));

            CreateMap<DeviceCreateDto, Device>()
              .ForMember(dest => dest.State, opt => opt.MapFrom(src => ParseState(src.State)));

            CreateMap<DeviceUpdateDto, Device>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => ParseState(src.State)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); 
        }

        private static DeviceState ParseState(string state)
        {
            return Enum.TryParse<DeviceState>(state, true, out var parsed)
                ? parsed
                : DeviceState.Inactive;
        }
    }
}
