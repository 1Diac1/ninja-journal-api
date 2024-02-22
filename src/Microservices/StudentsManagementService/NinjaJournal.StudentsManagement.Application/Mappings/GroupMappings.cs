using NinjaJournal.StudentsManagement.Domain.Entities;
using AutoMapper;
using NinjaJournal.StudentsManagement.Application.Dtos;

namespace NinjaJournal.StudentsManagement.Application.Mappings;

public class GroupMappings : Profile
{
    public GroupMappings()
    {
        CreateMap<Group, GroupDto>();
        CreateMap<GroupDto, Group>();
    }
}