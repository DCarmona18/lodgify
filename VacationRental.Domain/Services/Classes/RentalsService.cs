using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Helpers;
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
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public RentalsService(IRentalsRepository rentalsRepository, IMapper mapper, IBookingService bookingService)
        {
            _rentalsRepository = rentalsRepository;
            _mapper = mapper;
            _bookingService = bookingService;
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
            var data = await _rentalsRepository.CreateUpdate(rentalsEntity)
                                 ?? throw new ApplicationException("Error creating Rental");
            
            return new ResourceIdViewModel { Id = data.Id };
        }

        ///<inheritdoc/>
        public async Task<ResourceIdViewModel> UpdateAsync(int rentalId, RentalBindingModelV2 model)
        {
            var rental = await _rentalsRepository.GetById(rentalId);
            if (rental.Count == 0)
                throw new ApplicationException("Rental not found");

            if (model.PreparationTimeInDays > rental.First().Value.PreparationTimeInDays
                || model.Units < rental.First().Value.Units)
            {
                // Validate conflict
                var bookings = await _bookingService.GetBookingsByRentalId(rentalId);
                foreach (var booking in bookings)
                {
                    if (await _bookingService.ValidateOverLapping(booking, model, bookings))
                        throw new ApplicationException("Dates overlap if performs this action");
                }
            }

            // Update model
            var rentalEntity = _mapper.Map<RentalsEntity>(model);
            rentalEntity.Id = rentalId;
            rentalEntity = await _rentalsRepository.CreateUpdate(rentalEntity);

            return new ResourceIdViewModel { Id = rentalEntity.Id };
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
