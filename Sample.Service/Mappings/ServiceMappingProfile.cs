using AutoMapper;
using Sample.Repository.Models;
using Sample.Service.DTOs;

namespace Sample.Service.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<SchoolInfoModel, SchoolInfoDto>();
        }
    }
}