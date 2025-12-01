using AutoMapper;
using SlotIQ.Interview.API.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.API.Configuration;

/// <summary>
/// AutoMapper profile for API request/response models to DTOs
/// </summary>
public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateMemberRequest, CreateMemberDto>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

        CreateMap<UpdateMemberRequest, UpdateMemberDto>()
            .ForMember(dest => dest.EmailID, opt => opt.MapFrom(src => src.EmailAddress))
            .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.RoleName))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.UpdatedBy));
    }
}
