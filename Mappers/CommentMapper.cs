using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };

        }
        public static Comment ToCreateCommentDto(this CreateCommentDTO commentDto)
        {
            return new Comment
            {
                Content = commentDto.Content,
                Title = commentDto.Title,
                StockId = commentDto.StockId
            };
        }
    }
}