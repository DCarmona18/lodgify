using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Services.Interfaces
{
    public interface IBookingService
    {
        /// <summary>
        /// Gets booking information given an Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<BookingViewModel> GetByIdAsync(int bookingId);
        /// <summary>
        /// Creates a Booking
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResourceIdViewModel> CreateAsync(BookingBindingModel model);
        /// <summary>
        /// Gets a list of the Booking data given a rental Id
        /// </summary>
        /// <param name="rentalId"></param>
        /// <returns></returns>
        Task<List<BookingViewModel>> GetBookingsByRentalId(int rentalId);
        /// <summary>
        /// Validates if a model X overlaps with a model Y in a specific rental 
        /// (Validates also the units)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rental"></param>
        /// <returns></returns>
        Task<bool> ValidateOverLapping(BookingViewModel model, RentalBindingModelV2 rental, List<BookingViewModel> bookings);
    }
}
