using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles;

//Profile -> comes from AutoMapper Namespace
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //Map from LeaveType to LeaveTypeReadOnlyVM
        CreateMap<LeaveType, LeaveTypeReadOnlyVM>()
            //Names of 2 properties that should be mapped are different
            //dest -> destination property
            //opt -> option; src -> source
            .ForMember(dest => dest.Days,
                       opt => 
                       opt.MapFrom(src => src.NumberOfDays));

        //Map from LeaveTypeCreateVM to LeaveType
        //LeaveTypeCreateVM -> comes from a Form (Create View)
        CreateMap<LeaveTypeCreateVM, LeaveType>()
            .ForMember(dest => dest.NumberOfDays,
                       opt =>
                       opt.MapFrom(src => src.Days));

        //Map from LeaveType to LeaveTypeEditVM and other way around
        //LeaveType to LeaveTypeEditVM -> generate Edit View
        //LeaveTypeEditVM to LeaveType -> comes from a Form (Edit View)
        CreateMap<LeaveType, LeaveTypeEditVM>()
            .ForMember(dest => dest.Days,
                       opt =>
                       opt.MapFrom(src => src.NumberOfDays)).ReverseMap();
    }
}

