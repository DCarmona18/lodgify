using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services.Interfaces;

namespace VacationRental.Domain.Services.Classes
{
    public class VacationsRentalCalendarService : IVacationsRentalCalendarService
    {
        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Public Methods
        public Task<CalendarViewModel> GetAvailabilityAsync(int rentalId, DateTime start, int nights)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
