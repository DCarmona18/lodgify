using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Infrastructure.Entities;
using VacationRental.Infrastructure.Repositories.Interfaces;

namespace VacationRental.Infrastructure.Repositories.Classes
{
    public class BookingRepository : IBookingRepository
    {
        #region Properties
        private readonly IDictionary<int, BookingEntity> _bookings;
        #endregion

        #region Constructor
        public BookingRepository(IDictionary<int, BookingEntity> bookings)
        {
            _bookings = bookings;
        }
        #endregion

        #region Public Methods
        /// <inheritdoc/>
        public async Task<IDictionary<int, BookingEntity>> GetById(int id) 
        {
            return await Task.Run(() => {
                return _bookings.Where(p => p.Key == id)
                    .ToDictionary(p => p.Key, p => p.Value);
            });
        }

        /// <inheritdoc/>
        public async Task<IDictionary<int, BookingEntity>> GetAll()
        {
            return await Task.Run(() => {
                return _bookings;
            });
        }

        public async Task<BookingEntity> CreateUpdate(BookingEntity bookingEntity)
        {
            BookingEntity result = null;
            if (bookingEntity.Id > 0)
            {
                // Update
                var rental = await GetById(bookingEntity.Id);
                if (rental != null)
                    result = _bookings[bookingEntity.Id] = bookingEntity;
            }
            else
            {
                // Create
                int newId = GetLastestId();
                bookingEntity.Id = newId;
                _bookings.Add(newId, bookingEntity);
                result = (await GetById(newId)).First().Value;
            }

            return result;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets Latest Id to Insert
        /// </summary>
        /// <returns></returns>
        private int GetLastestId() =>
            _bookings.Keys.Count + 1;
        #endregion
    }
}
