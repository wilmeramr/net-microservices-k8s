using AutoMapper;
using PlatFormService.Dtos;
using PlatFormService.Models;

namespace PlatFormService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();

        }

    }
}