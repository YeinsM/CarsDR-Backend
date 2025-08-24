using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Extensions;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IListingRepository listingRepository,
        IPaginationService paginationService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
        {
            Guid userId = User.GetUserId();
            Domain.Entities.User? user = await userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return Unauthorized(ApiResponseBuilder.Fail<string>(401, "Invalid user"));
            }

            Domain.Entities.Listing? listing = await listingRepository.GetByIdAsync(request.ListingId);
            if (listing is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Listing not found"));
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = request.Text,
                UserId = userId,
                ListingId = request.ListingId,
                CreatedAt = DateTime.UtcNow
            };

            await commentRepository.CreateAddAsync(comment);
            await commentRepository.SaveChangesAsync();

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
        [Authorize(Policy = "AdminOrUser")]
        public async Task<ActionResult<PaginatedResponse<CommentResponse>>> GetByListing(
        Guid listingId,
            [FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            IQueryable<Comment> query = commentRepository.QueryByListingId(listingId);

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            PaginatedResponse<CommentResponse> paginatedResult = await paginationService.PaginateAsync(
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
        [Authorize(Policy = "AdminOrUser")]
        public async Task<ActionResult<PaginatedResponse<CommentResponse>>> GetByUser(
            Guid userId,
            [FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            IQueryable<Comment> query = commentRepository.QueryByUserId(userId);

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            PaginatedResponse<CommentResponse> paginatedResult = await paginationService.PaginateAsync(
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
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Comment? comment = await commentRepository.GetByIdAsync(id);
            if (comment is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Comment not found."));
            }

            await commentRepository.DeleteAsync(comment);
            await commentRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Comment deleted successfully"));
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentRequest request)
        {
            Comment? comment = await commentRepository.GetByIdAsync(id);
            if (comment is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Comment not found."));
            }

            comment.Content = request.Content;
            await commentRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Comment updated successfully"));
        }

        [HttpPatch("{id}/report")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Report(Guid id)
        {
            Comment? comment = await commentRepository.GetByIdAsync(id);
            if (comment is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Comment not found."));
            }

            comment.IsReported = true;
            await commentRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Comment reported successfully"));
        }
    }
}
