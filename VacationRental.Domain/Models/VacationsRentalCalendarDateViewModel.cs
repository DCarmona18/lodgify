using System;
using System.Collections.Generic;

namespace VacationRental.Domain.Models
{
    public class VacationsRentalCalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<VacationsRentalCalendarBookingViewModel> Bookings { get; set; }
        public List<PreparationTimesViewModel> PreparationTimes { get; set; }
    }
}