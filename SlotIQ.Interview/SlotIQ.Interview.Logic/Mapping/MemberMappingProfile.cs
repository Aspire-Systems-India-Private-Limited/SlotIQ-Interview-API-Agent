using AutoMapper;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Mapping;

public class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        // Entity to DTO
        CreateMap<Member, MemberDto>()
            .ForMember(d => d.RoleName, o => o.MapFrom(s => (MemberRole)s.RoleID))
            .ForMember(d => d.Source, o => o.MapFrom(s => (Source)int.Parse(s.Source)))
            .ForMember(d => d.ModUser, o => o.MapFrom(s => s.ModifiedBy));

        // DTO to Entity
        CreateMap<MemberDto, Member>()
            .ForMember(d => d.RoleID, o => o.MapFrom(s => (int)s.RoleName))
            .ForMember(d => d.Source, o => o.MapFrom(s => ((int)s.Source).ToString()))
            .ForMember(d => d.ModifiedBy, o => o.MapFrom(s => s.ModUser));

        // CreateDTO to Entity
        CreateMap<CreateMemberDto, Member>()
            .ForMember(d => d.MemberID, o => o.Ignore())
            .ForMember(d => d.RoleID, o => o.MapFrom(s => (int)s.RoleName))
            .ForMember(d => d.EmailID, o => o.MapFrom(s => s.EmailAddress))
            .ForMember(d => d.Source, o => o.MapFrom(s => ((int)s.Source).ToString()))
            .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.UpdatedBy))
            .ForMember(d => d.ModifiedBy, o => o.MapFrom(s => s.UpdatedBy))
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.ModifiedDate, o => o.Ignore());
    }
}
