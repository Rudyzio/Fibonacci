using AutoMapper;
using Fibonacci.Entities;
using Fibonacci.Models;

namespace Fibonacci.MapperProfile
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<HistoricEntry, HistoricEntryDto>();
        }
    }
}