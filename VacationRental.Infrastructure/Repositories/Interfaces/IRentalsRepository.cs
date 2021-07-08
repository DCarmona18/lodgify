using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Infrastructure.Entities;

namespace VacationRental.Infrastructure.Repositories.Interfaces
{
    public interface IRentalsRepository
    {
        /// <summary>
        /// Get Rental data given an Id
        /// </summary>
        /// <param name="rentalId"></param>
        /// <returns></returns>
        Task<IDictionary<int, RentalsEntity>> GetById(int rentalId);
        /// <summary>
        /// Creates or Update a Rental
        /// </summary>
        /// <param name="rentalsEntity"></param>
        /// <returns></returns>
        Task<RentalsEntity> CreateUpdate(RentalsEntity rentalsEntity);
    }
}
