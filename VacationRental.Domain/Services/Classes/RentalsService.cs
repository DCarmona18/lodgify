using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services.Interfaces;
using VacationRental.Infrastructure.Entities;
using VacationRental.Infrastructure.Repositories.Interfaces;

namespace VacationRental.Domain.Services.Classes
{
    public class RentalsService : IRentalsService
    {
        #region Properties
        private readonly IRentalsRepository _rentalsRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public RentalsService(IRentalsRepository rentalsRepository, IMapper mapper)
        {
            _rentalsRepository = rentalsRepository;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        ///<inheritdoc/>
        public async Task<T> GetByIdAsync<T>(int rentalId)
        {
            var rental = await _rentalsRepository.GetById(rentalId);
            if (rental.Count == 0)
                throw new ApplicationException("Rental not found");

            return _mapper.Map<T>(rental.First().Value);
        }
        ///<inheritdoc/>
        public async Task<ResourceIdViewModel> CreateAsync(IRentalBinding model)
        {
            var rentalsEntity = _mapper.Map<RentalsEntity>(model);
            var data = _mapper.Map<RentalViewModel>(await _rentalsRepository.CreateUpdate(rentalsEntity))
                                 ?? throw new ApplicationException("Error creating Rental");
            
            return new ResourceIdViewModel { Id = data.Id };
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
