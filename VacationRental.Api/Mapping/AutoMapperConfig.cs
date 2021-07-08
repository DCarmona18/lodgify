using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using VacationRental.Infrastructure.Entities;

namespace VacationRental.Api.Mapping
{
    [ExcludeFromCodeCoverage]
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<BookingEntity, BookingViewModel>().ReverseMap();
            CreateMap<BookingViewModel, BookingEntity>().ReverseMap();

            CreateMap<RentalsEntity, RentalViewModel>().ReverseMap();
            CreateMap<RentalViewModel, RentalsEntity>().ReverseMap();
            
            CreateMap<RentalBindingModel, RentalsEntity>().ReverseMap();

            CreateMap<BookingBindingModel, BookingViewModel>().ReverseMap();
        }
    }
}
