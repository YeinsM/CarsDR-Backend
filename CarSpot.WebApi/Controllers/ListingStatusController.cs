using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;



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
        public async Task<ActionResult<IEnumerable<ListingStatus>>> Get()
        {
            var statuses = await _repository.GetAllAsync();
            return Ok(statuses);
        }
    }
