using NinjaJournal.IdentityService.Application.Dtos;
using NinjaJournal.IdentityService.Domain.Entities;
using AutoMapper;

namespace NinjaJournal.IdentityService.Application.Mappings;

public class ApplicationRoleMappings : Profile
{
    public ApplicationRoleMappings()
    {
        CreateMap<ApplicationRole, ApplicationRoleDto>();
        CreateMap<ApplicationRoleDto, ApplicationRole>();
    }
}