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
    }
}
