using NinjaJournal.StudentsManagement.Application.Dtos;
using NinjaJournal.StudentsManagement.Domain.Entities;
using AutoMapper;

namespace NinjaJournal.StudentsManagement.Application.Mappings;

public class GroupMappings : Profile
{
    public GroupMappings()
    {
        CreateMap<Group, GroupDto>();
        CreateMap<GroupDto, Group>();
    }
}