using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;
        public CommentController(ICommentService commentService,
                                ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var commentDto = await _commentService.GetAllAsync();
            return Ok(commentDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(id);
                if (comment == null)
                {
                    return NotFound();
                }
                return Ok(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByIdAsync da xato: {Id}", id);
                return StatusCode(500, "Server xatosi");
            }

        }

        #region  Create Comment
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO createCommentDTO)
        {

            var commentModel = await _commentService.CreateCommentAsync(createCommentDTO);
            _logger.LogInformation($"Comment yaratildi:{commentModel.Id}");
            return Ok(commentModel);
        }
        #endregion
        #region Update Comment
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchComment([FromRoute] int id, [FromBody] UpdateCommentDTO updateCommentDTO)
        {
            var commentModel = await _commentService.UpdateCommentAsync(id, updateCommentDTO);
            return Ok(commentModel);
        }
        #endregion Update Comment
    }
    
}