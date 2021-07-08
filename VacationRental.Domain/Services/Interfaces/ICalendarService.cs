﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Services.Interfaces
{
    public interface ICalendarService
    {
        /// <summary>
        /// Gets Availability of a Rental
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="start"></param>
        /// <param name="nights"></param>
        /// <returns></returns>
        Task<CalendarViewModel> GetAvailabilityAsync(int rentalId, DateTime start, int nights);
    }
}
