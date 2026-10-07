using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment(
        [FromBody] CreateCommentDto request)
    {
        Comment comment = new Comment
        {
            Body = request.Body,
            PostId = request.PostId,
            UserId = request.UserId
        };

        Comment created = await commentRepository.AddAsync(comment);

        CommentDto dto = new CommentDto
        {
            Id = created.Id,
            Body = created.Body,
            PostId = created.PostId,
            UserId = created.UserId
        };

        return Created($"/Comments/{dto.Id}", dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? postId,
        [FromQuery] int? userId)
    {
        IQueryable<Comment> comments = commentRepository.GetMany();

        if (postId.HasValue)
        {
            comments = comments.Where(comment =>
                comment.PostId == postId.Value);
        }

        if (userId.HasValue)
        {
            comments = comments.Where(comment =>
                comment.UserId == userId.Value);
        }

        IEnumerable<CommentDto> result = comments.Select(comment =>
            new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            });

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        try
        {
            Comment comment =
                await commentRepository.GetSingleAsync(id);

            CommentDto dto = new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            };

            return Ok(dto);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment(
        int id,
        [FromBody] CommentDto request)
    {
        if (id != request.Id)
        {
            return BadRequest("The id in the URL does not match the comment id.");
        }

        Comment comment = new Comment
        {
            Id = request.Id,
            Body = request.Body,
            PostId = request.PostId,
            UserId = request.UserId
        };

        try
        {
            await commentRepository.UpdateAsync(comment);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            await commentRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}