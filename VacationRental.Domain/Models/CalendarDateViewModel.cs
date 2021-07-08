﻿using System;
using System.Collections.Generic;

namespace VacationRental.Domain.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<PreparationTimesViewModel> PreparationTimes { get; set; }
    }
}
