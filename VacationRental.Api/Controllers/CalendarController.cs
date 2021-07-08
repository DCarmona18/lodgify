using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Obsolete]
    public class CalendarController : ControllerBase
    {
        #region Properties
        private readonly ICalendarService _calendarService;
        #endregion

        #region Constructor
        public CalendarController(
            ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public async Task<CalendarViewModel> GetAsync(int rentalId, DateTime start, int nights) =>
            await _calendarService.GetAvailabilityAsync(rentalId, start, nights);
        #endregion
    }
}
