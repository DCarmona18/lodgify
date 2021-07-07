using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        #region Properties
        private readonly IBookingService _bookingService;
        #endregion

        #region Constructor
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> GetAsync(int bookingId) =>
            await _bookingService.GetByIdAsync(bookingId);

        [HttpPost]
        public async Task<ResourceIdViewModel> PostAsync(BookingBindingModel model) => 
            await _bookingService.CreateAsync(model);
        #endregion
    }
}
