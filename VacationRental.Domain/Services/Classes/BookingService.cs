using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Helpers;
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

            /**/
            var rental = _mapper.Map<RentalBindingModel>(rentalEntity.First().Value);
            var bookingViewModel = _mapper.Map<BookingViewModel>(model);
            var bookings = await GetBookingsByRentalId(model.RentalId);
            if (await ValidateOverLapping(bookingViewModel, rental, bookings))
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

        ///<inheritdoc/>
        public async Task<bool> ValidateOverLapping(BookingViewModel bookingModel, RentalBindingModel rental, List<BookingViewModel> bookings) 
        {
            var response = false;
            int preparationDays = rental.PreparationTimeInDays;

            var count = 0;
            foreach (var booking in bookings)
            {
                var modelEndDate = bookingModel.Start.AddDays(bookingModel.Nights + preparationDays);
                var bookingEndDate = booking.Start.AddDays(booking.Nights + preparationDays);

                if (booking.Id != bookingModel.Id &&
                    DatesHelper.TimesOverlap(booking.Start, bookingEndDate, bookingModel.Start, modelEndDate))
                    count++;
            }

            if (count >= rental.Units)
                response = true;

            return response;
        }

        ///<inheritdoc/>
        public async Task<List<BookingViewModel>> GetBookingsByRentalId(int rentalId)
        {
            var result = new List<BookingViewModel>();
            var bookings = _mapper.Map<IDictionary<int, BookingViewModel>>(await _bookingsRepository.GetAll());
            foreach (var booking in bookings.Values)
            {
                if (booking.RentalId == rentalId)
                    result.Add(booking);
            }

            return result;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
