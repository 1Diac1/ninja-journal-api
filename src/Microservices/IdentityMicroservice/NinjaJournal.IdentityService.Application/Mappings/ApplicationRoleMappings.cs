using NinjaJournal.IdentityService.Application.Abstractions.Dtos;
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