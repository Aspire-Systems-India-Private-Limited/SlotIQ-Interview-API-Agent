using AutoMapper;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Mapping;

/// <summary>
/// AutoMapper profile for Member entity and DTOs
/// </summary>
public class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        // Entity to DTO mappings
        CreateMap<Member, MemberDto>();

        // DTO to Entity mappings
        CreateMap<CreateMemberDto, Member>()
            .ForMember(dest => dest.MemberID, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

        CreateMap<UpdateMemberDto, Member>()
            .ForMember(dest => dest.MemberID, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

        CreateMap<MemberDto, Member>();
    }
}
