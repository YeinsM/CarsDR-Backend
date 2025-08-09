using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IListingRepository _listingRepository;

        public CommentsController(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IListingRepository listingRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _listingRepository = listingRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "User not found"));

            var listing = await _listingRepository.GetByIdAsync(request.ListingId);
            if (listing is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Listing not found"));

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = request.Text,
                UserId = request.UserId,
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
        public async Task<IActionResult> GetByListing(
            Guid listingId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 100)
        {
            const int maxPageSize = 100;

            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 1;
            else if (pageSize > maxPageSize) pageSize = maxPageSize;

            var query = _commentRepository.QueryByListingId(listingId);

            var total = await query.CountAsync();

            var pagedComments = await query
                .OrderBy(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paged = pagedComments.Select(c => new CommentResponse(
                c.Id,
                c.Content ?? "",
                c.UserId,
                c.User?.FullName ?? "Unknown",
                c.CreatedAt,
                c.IsReported,
                new List<CommentResponse>()
            )).ToList();

            var result = new
            {
                TotalRecords = total,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = paged
            };

            return Ok(ApiResponseBuilder.Success(result, "Comments retrieved successfully"));
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(
            Guid userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 100)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            else if (pageSize > 100) pageSize = 100; 

        
            var query = _commentRepository.QueryByUserId(userId);

          
            var totalRecords = await query.CountAsync();

            
            var pagedComments = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var responseItems = pagedComments.Select(c => new CommentResponse(
                c.Id,
                c.Content ?? "",
                c.UserId,
                c.Listing?.Title ?? "Unknown listing",
                c.CreatedAt,
                c.IsReported,
                new List<CommentResponse>()
            ));

            var result = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = responseItems
            };

            return Ok(ApiResponseBuilder.Success(result, "User comments retrieved successfully"));
        }



        [HttpDelete("{id}")]
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
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentRequest request)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Comment not found."));

            comment.Content = request.Content;
            await _commentRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Comment updated successfully"));
        }
    }
}
