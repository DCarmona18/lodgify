﻿using System;
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
    [Obsolete]
    public class RentalsController : ControllerBase
    {
        #region Properties
        private readonly IRentalsService _rentalsService;
        #endregion

        #region Constructor
        public RentalsController(IRentalsService rentalsService)
        {
            _rentalsService = rentalsService;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> GetAsync(int rentalId) => 
            await _rentalsService.GetByIdAsync<RentalViewModel>(rentalId);

        [HttpPost]
        public async Task<ResourceIdViewModel> PostAsync(RentalBindingModel model) =>
            await _rentalsService.CreateAsync(model);
        #endregion
    }
}
