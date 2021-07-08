using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Infrastructure.Entities;

namespace VacationRental.Infrastructure.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        /// <summary>
        /// Get Booking data given an Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IDictionary<int, BookingEntity>> GetById(int id);
        /// <summary>
        /// Creates or Update a Booking
        /// </summary>
        /// <param name="bookingEntity"></param>
        /// <returns></returns>
        Task<BookingEntity> CreateUpdate(BookingEntity bookingEntity);
        /// <summary>
        /// Gets all the booking in repository
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<int, BookingEntity>> GetAll();
    }
}
