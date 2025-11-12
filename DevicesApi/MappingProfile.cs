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

            CreateMap<DeviceCreateDto, Device>();

            //.ForMember(dest => dest.State, opt => opt.MapFrom(src =>
            //    Enum.TryParse<DeviceState>(src.State, true, out var state) ? state : DeviceState.Inactive));
            CreateMap<DeviceUpdateDto, Device>()
                //.ForMember(dest => dest.State, opt => opt.MapFrom(src =>
                //    Enum.TryParse<DeviceState>(src.State, true, out var state) ? state : DeviceState.Off))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); 
        }
    }
}
