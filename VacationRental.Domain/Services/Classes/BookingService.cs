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
    public class BookingService : IBookingService
    {
        #region Properties
        private readonly IBookingRepository _bookingsRepository;
        private readonly IRentalsRepository _rentalsRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public BookingService(
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
        ///<inheritdoc/>
        public async Task<BookingViewModel> GetByIdAsync(int bookingId)
        {
            var bookingEntity = await _bookingsRepository.GetById(bookingId);
            if (bookingEntity.Count == 0)
                throw new ApplicationException("Booking not found");

            return _mapper.Map<BookingViewModel>(bookingEntity.First().Value);
        }

        ///<inheritdoc/>
        public async Task<ResourceIdViewModel> CreateAsync(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be positive");

            var rentalEntity = await _rentalsRepository.GetById(model.RentalId);
            if (rentalEntity.Count == 0)
                throw new ApplicationException("Rental not found");

            int preparationDays = rentalEntity.First().Value.PreparationTimeInDays;
            var bookings = _mapper.Map<IDictionary<int, BookingViewModel>>(await _bookingsRepository.GetAll());
            
            var count = 0;
            foreach (var booking in bookings.Values)
            {
                var modelEndDate = model.Start.AddDays(model.Nights + preparationDays);
                var bookingEndDate = booking.Start.AddDays(booking.Nights + preparationDays);

                if (booking.RentalId == model.RentalId && TimeOverlaps(booking.Start, bookingEndDate, model.Start, modelEndDate))
                    count++;
            }

            if (count >= rentalEntity.First().Value.Units)
                throw new ApplicationException("Not available");

            var bookingEntity = new BookingEntity
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            };
            var result = await _bookingsRepository.CreateUpdate(bookingEntity);

            return new ResourceIdViewModel { Id = result.Id };
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Validates if 2 Dates overlap each other
        /// </summary>
        /// <param name="startX">Date 1: Start Date</param>
        /// <param name="endX">Date 1: End Date</param>
        /// <param name="startY">Date 2: Start Date</param>
        /// <param name="endY">Date 2: End Date</param>
        /// <returns></returns>
        private bool TimeOverlaps(DateTime startX, DateTime endX, DateTime startY, DateTime endY) 
        {
            return ((startX <= startY.Date && endX > startY.Date)
                    || (startX < endY && endX >= endY)
                    || (startX > startY && endX < endY));
        }
        #endregion
    }
}
