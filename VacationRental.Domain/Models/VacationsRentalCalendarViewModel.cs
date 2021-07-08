using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Domain.Models
{
    public class VacationsRentalCalendarViewModel
    {
        public int RentalId { get; set; }
        public List<VacationsRentalCalendarDateViewModel> Dates { get; set; }
    }
}
