using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;






[ApiController]
[Route("api/[controller]")]
public class PublicationsController : ControllerBase
{
    private readonly IPublicationRepository _publicationRepository;
    private readonly IMapper _mapper;

    public PublicationsController(IPublicationRepository publicationRepository, IMapper mapper)
    {
        _publicationRepository = publicationRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var publications = await _publicationRepository.GetAllAsync();
        var response = publications.Select(p => new PublicationResponse
        {
            Id = p.Id,
            Make = p.Make.Name,
            Model = p.Model.Name,
            Color = p.Color.Name,
            Price = p.Price,
            Currency = p.Currency,
            Place = p.Place,
            Version = p.Version,
            Images = p.Images,
            CreatedAt = p.CreatedAt
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var publication = await _publicationRepository.GetByIdAsync(id);
        if (publication is null) return NotFound();

        var response = new PublicationResponse
        {
            Id = publication.Id,
            Make = publication.Make.Name,
            Model = publication.Model.Name,
            Color = publication.Color.Name,
            Price = publication.Price,
            Currency = publication.Currency,
            Place = publication.Place,
            Version = publication.Version,
            Images = publication.Images,
            CreatedAt = publication.CreatedAt
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePublicationRequest request)
    {
        var publication = new Publication(
            request.UserId,
            request.MakeId,
            request.ModelId,
            request.ColorId,
            request.Price,
            request.Currency,
            request.Place,
            request.Version,
            request.Images
        );

        await _publicationRepository.AddAsync(publication);
        return CreatedAtAction(nameof(GetById), new { id = publication.Id }, publication.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CreatePublicationRequest request)
    {
        var publication = await _publicationRepository.GetByIdAsync(id);
        if (publication is null) return NotFound();

      


        publication = new Publication(
            request.UserId,
            request.MakeId,
            request.ModelId,
            request.ColorId,
            request.Price,
            request.Currency,
            request.Place,
            request.Version,
            request.Images
        );

        typeof(Publication).GetProperty("Id")!.SetValue(publication, id);
        await _publicationRepository.UpdateAsync(publication);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var publication = await _publicationRepository.GetByIdAsync(id);
        if (publication is null) return NotFound();

        await _publicationRepository.DeleteAsync(publication);
        return NoContent();
    }
}
