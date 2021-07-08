using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vacationrental/rentals")]
    [ApiController]
    public class VacationsRentalController : ControllerBase
    {
        #region Properties
        private readonly IRentalsService _rentalsService;
        #endregion

        #region Constructor
        public VacationsRentalController(IRentalsService rentalsService)
        {
            _rentalsService = rentalsService;
        }
        #endregion

        #region Public Methods

        [HttpPost]
        public async Task<ResourceIdViewModel> PostAsync(RentalBindingModel model) =>
            await _rentalsService.CreateAsync(model);

        [HttpPut("{rentalId:int}")]
        public async Task<ResourceIdViewModel> PutAsync(int rentalId, RentalBindingModel model) =>
            await _rentalsService.UpdateAsync(rentalId, model);
        #endregion
    }
}
