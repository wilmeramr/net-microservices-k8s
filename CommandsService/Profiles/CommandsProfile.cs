using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatFormService;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(p => p.ExternalID, opt => opt.MapFrom(src => src.Id));
                CreateMap<GrpcPlatformModel,Platform>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                  .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}