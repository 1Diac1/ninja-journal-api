using NinjaJournal.IdentityService.Domain.Entities;
using AutoMapper;
using NinjaJournal.IdentityService.Application.Dtos;

namespace NinjaJournal.IdentityService.Application.Mappings;

public class ApplicationRoleMappings : Profile
{
    public ApplicationRoleMappings()
    {
        CreateMap<ApplicationRole, ApplicationRoleDto>();
        CreateMap<ApplicationRoleDto, ApplicationRole>();
    }
}