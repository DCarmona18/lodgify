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

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModelV2> GetAsync(int rentalId) =>
            await _rentalsService.GetByIdAsync<RentalViewModelV2>(rentalId);

        [HttpPost]
        public async Task<ResourceIdViewModel> PostAsync(RentalBindingModelV2 model) =>
            await _rentalsService.CreateAsync(model);
        #endregion
    }
}
