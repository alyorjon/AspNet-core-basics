using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Service
{
    public class CommentService : ICommentService
    {
        #region Constructor
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        #endregion


        #region  Get all comments
        public async Task<List<CommentDto>> GetAllAsync()
        {
            var comments = await _commentRepository.GetAllsAsync();
            return comments.Select(s => s.ToCommentDto()).ToList();
        }
        #endregion

        #region  Get comment by id
        public Task<CommentDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion Get comment by id

        #region Create new Comment
        public async Task<CommentDto> CreateCommentAsync(CreateCommentDTO createCommentDTO)
        {
            try
            {
                if (createCommentDTO == null)
                    throw new ArgumentNullException(nameof(createCommentDTO), "CommentDto bo'sh bo'lishi mumkin emas");
                if (string.IsNullOrWhiteSpace(createCommentDTO.Title))
                    throw new ArgumentException("Title bo'sh bo'lishi mumkin emas", nameof(createCommentDTO.Title));
                if (string.IsNullOrWhiteSpace(createCommentDTO.Content))
                    throw new ArgumentException("Content bo'sh bo'lishi mumkin emas", nameof(createCommentDTO.Content));
                if (createCommentDTO.StockId <= 0)
                    throw new ArgumentException("StockId noldan kichik yoki  bo'sh bo'lishi mumkin emas", nameof(createCommentDTO.StockId));
                var commentModel = createCommentDTO.ToCreateCommentDto();
                var createdComment = await _commentRepository.CreateAsync(commentModel);
                return createdComment.ToCommentDto();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Xatolik chiqdi");
            }
            // throw new NotImplementedException();
        }
        #endregion
        #region Patch Comment
        public async Task<CommentDto> UpdateCommentAsync(int id,UpdateCommentDTO updateCommentDTO)
        {
            try
            {
                var exists = await _commentRepository.GetByIdAsync(id);
                if (exists == null)
                    throw new ArgumentException("Comment mavjud emas");
                if (updateCommentDTO == null)
                    throw new ArgumentNullException(nameof(updateCommentDTO), "CommentDto bo'sh bo'lishi mumkin emas");
                if (string.IsNullOrWhiteSpace(updateCommentDTO.Title))
                    throw new ArgumentException("Title bo'sh bo'lishi mumkin emas", nameof(updateCommentDTO.Title));
                if (string.IsNullOrWhiteSpace(updateCommentDTO.Content))
                    throw new ArgumentException("Content bo'sh bo'lishi mumkin emas", nameof(updateCommentDTO.Content));
                if (updateCommentDTO.StockId <= 0)
                    throw new ArgumentException("StockId noldan kichik yoki  bo'sh bo'lishi mumkin emas", nameof(updateCommentDTO.StockId));
                return exists.ToCommentDto();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Xatolik chiqdi");
            }
        }
        #endregion
    }
}