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
        }
    }
}
