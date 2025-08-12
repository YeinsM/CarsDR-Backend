
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingStatusController : ControllerBase
    {
        private readonly IListingStatusRepository _repository;

        public ListingStatusController(IListingStatusRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrCompany")]
        public async Task<IActionResult> Get()
        {
            var statuses = await _repository.GetAllAsync();
            return Ok(ApiResponseBuilder.Success(statuses, "Listing statuses retrieved successfully."));
        }
    }
}
