using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services.Interfaces;
using VacationRental.Infrastructure.Entities;
using VacationRental.Infrastructure.Repositories.Interfaces;

namespace VacationRental.Domain.Services.Classes
{
    public class CalendarService : ICalendarService
    {
        #region Properties
        private readonly IBookingRepository _bookingsRepository;
        private readonly IRentalsRepository _rentalsRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public CalendarService(
            IBookingRepository bookingsRepository,
            IRentalsRepository rentalsRepository,
            IMapper mapper)
        {
            _bookingsRepository = bookingsRepository;
            _rentalsRepository = rentalsRepository;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        /// <inheritdoc/>
        public async Task<CalendarViewModel> GetAvailabilityAsync(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rentals = _mapper.Map<IDictionary<int, RentalViewModel>>(await _rentalsRepository.GetById(rentalId));
            if (rentals.Count == 0)
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = rentals.First().Value.Id,
                Dates = new List<CalendarDateViewModel>()
            };

            var bookings = await _bookingsRepository.GetAll();
            int preparationDays = rentals.First().Value.PreparationTimeInDays;
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimesViewModel>()
                };

                var results = bookings.Values
                                .Where(booking => booking.RentalId == rentalId &&
                                booking.Start <= date.Date &&
                                booking.Start.AddDays(booking.Nights + preparationDays) > date.Date).ToList();

                int unitNumber = 1;
                results.ForEach(booking =>
                {
                    if (start.Date.AddDays(i) >= booking.Start.AddDays(booking.Nights)
                        && start.Date.AddDays(i) <= booking.Start.AddDays(booking.Nights + preparationDays))
                        date.PreparationTimes.Add(new PreparationTimesViewModel { Unit = unitNumber++ });
                    else
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = unitNumber++ });
                });

                result.Dates.Add(date);
            }

            return result;
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
