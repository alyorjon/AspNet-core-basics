using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.DTOs.Comment;

namespace api.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetAllAsync();
        Task<CommentDto> GetByIdAsync(int id);
        Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDTO updateCommentDTO);

        Task<CommentDto> CreateCommentAsync(CreateCommentDTO createCommentDTO);
        
    }
}