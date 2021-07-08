using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vacationrental/calendar")]
    [ApiController]
    public class VacationsCalendarController : ControllerBase
    {
        #region Properties
        private readonly IVacationsRentalCalendarService _calendarService;
        #endregion

        #region Constructor
        public VacationsCalendarController(
            IVacationsRentalCalendarService calendarService)
        {
            _calendarService = calendarService;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public async Task<VacationsRentalCalendarViewModel> GetAsync(int rentalId, DateTime start, int nights) =>
            await _calendarService.GetAvailabilityAsync(rentalId, start, nights);
        #endregion
    }
}
