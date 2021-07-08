﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services.Classes;

namespace VacationRental.Domain.Services.Interfaces
{
    public interface IRentalsService
    {
        /// <summary>
        /// Gets Rental information given an Id
        /// </summary>
        /// <param name="rentalId"></param>
        /// <returns></returns>
        Task<RentalViewModel> GetByIdAsync(int rentalId);
        /// <summary>
        /// Creates a Rental
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResourceIdViewModel> CreateAsync(RentalBindingModel model);
        /// <summary>
        /// Updates Rental information
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResourceIdViewModel> UpdateAsync(int rentalId, RentalBindingModel model);
    }
}
