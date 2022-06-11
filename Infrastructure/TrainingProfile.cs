using AutoMapper;
using JobsTrainer.DTOs;
using JobsTrainer.Models;

namespace JobsTrainer.Infrastructure
{
    public class TrainingProfile : Profile
    {
        public TrainingProfile()
        {
            CreateMap<TrainJob, TrainJobDto>().ReverseMap();
        }
    }
}
