using AutoMapper;
using SlotIQ.Interview.API.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.API.Mapping;

public class MemberApiMappingProfile : Profile
{
    public MemberApiMappingProfile()
    {
        CreateMap<CreateMemberRequest, CreateMemberDto>()
            .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Firstname))
            .ForMember(d => d.LastName, o => o.MapFrom(s => s.Lastname));
    }
}
