using AutoMapper;
using JobsTrainer.Core.DTOs;
using JobsTrainer.Models;

namespace JobsTrainer.Infrastructure
{
    public class TrainingProfile : Profile
    {
        public TrainingProfile()
        {
            CreateMap<TrainJob, TrainJobDto>().ReverseMap();

            CreateMap<Company, CompanyDto>().ReverseMap();

            CreateMap<ExcelLmia, LmiaInfo>()
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address.Trim()))
                .ForMember(x => x.ProvinceTerritory, opt => opt.MapFrom(y => y.ProvinceTerritory.Trim()))
                .ForMember(x => x.ProgramStream, opt => opt.MapFrom(y => y.ProgramStream.Trim()))
                .ForMember(x => x.Employer, opt => opt.MapFrom(y => y.Employer.Trim()));
        }
    }
}
