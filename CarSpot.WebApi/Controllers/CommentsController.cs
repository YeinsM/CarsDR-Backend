using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarSpot.Application.Common.Extensions;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IListingRepository _listingRepository;
        private readonly IPaginationService _paginationService;

        public CommentsController(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IListingRepository listingRepository,
            IPaginationService paginationService)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _listingRepository = listingRepository;
            _paginationService = paginationService;

        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
        {
            var userId = User.GetUserId();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
                return Unauthorized(ApiResponseBuilder.Fail<string>(401, "Invalid user"));

            var listing = await _listingRepository.GetByIdAsync(request.ListingId);
            if (listing is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Listing not found"));

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = request.Text,
                UserId = userId,
                ListingId = request.ListingId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.CreateAddAsync(comment);
            await _commentRepository.SaveChangesAsync();

            var response = new CommentResponse(
                comment.Id,
                comment.Content ?? "",
                comment.UserId,
                user.FullName,
                comment.CreatedAt,
                false,
                new List<CommentResponse>()
            );

            return Ok(ApiResponseBuilder.Success(response, "Comment created successfully"));
        }

        [HttpGet("listing/{listingId}")]
        public async Task<ActionResult<PaginatedResponse<CommentResponse>>> GetByListing(
    Guid listingId,
    [FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _commentRepository.QueryByListingId(listingId);

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CommentResponse(
                        c.Id,
                        c.Content ?? "",
                        c.UserId,
                        c.User != null ? c.User.FullName : "Unknown",
                        c.CreatedAt,
                        c.IsReported,
                        new List<CommentResponse>()
                    )),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }



        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<PaginatedResponse<CommentResponse>>> GetByUser(
    Guid userId,
    [FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _commentRepository.QueryByUserId(userId);

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CommentResponse(
                        c.Id,
                        c.Content ?? "",
                        c.UserId,
                        c.Listing != null ? c.Listing.Title : "Unknown listing",
                        c.CreatedAt,
                        c.IsReported,
                        new List<CommentResponse>()
                    )),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }



        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Comment not found."));

            await _commentRepository.DeleteAsync(comment);
            await _commentRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Comment deleted successfully"));
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "AdminOrOwner")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentRequest request)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Comment not found."));

            comment.Content = request.Content;
            await _commentRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Comment updated successfully"));
        }

        [HttpPatch("{id}/report")]
        [Authorize]
        public async Task<IActionResult> Report(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Comment not found."));

            comment.IsReported = true;
            await _commentRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Comment reported successfully"));
        }
    }
}
