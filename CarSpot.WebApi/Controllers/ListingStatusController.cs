
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingStatusController(IListingStatusRepository repository) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Get()
        {
            System.Collections.Generic.IEnumerable<Domain.Entities.ListingStatus> statuses = await repository.GetAllAsync();
            return Ok(ApiResponseBuilder.Success(statuses, "Listing statuses retrieved successfully."));
        }
    }
}
