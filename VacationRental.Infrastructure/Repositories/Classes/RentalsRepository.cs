using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Infrastructure.Entities;
using VacationRental.Infrastructure.Repositories.Interfaces;

namespace VacationRental.Infrastructure.Repositories.Classes
{
    public class RentalsRepository : IRentalsRepository
    {
        #region Properties
        private readonly IDictionary<int, RentalsEntity> _rentals;
        #endregion

        #region Constructor
        public RentalsRepository(IDictionary<int, RentalsEntity> rentals)
        {
            _rentals = rentals;
        }
        #endregion

        #region Public Methods
        /// <inheritdoc/>
        public async Task<IDictionary<int, RentalsEntity>> GetById(int rentalId)
        {
            return await Task.Run(() => {
                return _rentals.Where(p => p.Key == rentalId)
                    .ToDictionary(p => p.Key, p => p.Value);
            });
        }

        /// <inheritdoc/>
        public async Task<RentalsEntity> CreateUpdate(RentalsEntity rentalsEntity)
        {
            RentalsEntity result = null;
            if (rentalsEntity.Id > 0)
            {
                // Update
                var rental = await GetById(rentalsEntity.Id);
                if (rental != null)
                {
                    result = _rentals[rentalsEntity.Id] = rentalsEntity;
                }
            }
            else 
            {
                // Create
                int newId = GetLastestId();
                rentalsEntity.Id = newId;
                _rentals.Add(newId, rentalsEntity);

                result = (await GetById(newId)).First().Value;
            }

            return result;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets Latest Id to Insert
        /// </summary>
        /// <returns></returns>
        private int GetLastestId() => 
            _rentals.Keys.Count + 1;
        #endregion
    }
}
