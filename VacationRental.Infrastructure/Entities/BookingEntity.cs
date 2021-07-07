using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Infrastructure.Entities
{
    public class BookingEntity
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }
    }
}
