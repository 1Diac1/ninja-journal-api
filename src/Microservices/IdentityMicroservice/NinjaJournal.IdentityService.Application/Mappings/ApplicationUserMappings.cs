using NinjaJournal.IdentityService.Application.Abstractions.Dtos;
using NinjaJournal.IdentityService.Domain.Entities;
using AutoMapper;

namespace NinjaJournal.IdentityService.Application.Mappings;

public class ApplicationUserMappings : Profile
{
    public ApplicationUserMappings()
    {
        CreateMap<ApplicationUser, ApplicationUserDto>();
        CreateMap<ApplicationUserDto, ApplicationUser>();

        CreateMap<CreateApplicationUserDto, ApplicationUser>()
            .ForMember(u => u.PasswordHash, opt =>
                opt.MapFrom(u => u.Password));
    }
}