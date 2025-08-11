using System;
using api.ApplicationDbContext;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository:ICommentRepository
{
    private readonly ApplicationDBContext _context;
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Comment>> GetAllsAsync()
    {
        return await _context.Comment.ToListAsync();
    }

    public async Task<Comment> GetByIdAsync([FromRoute] int id)
    {
        var comment = await _context.Comment.FindAsync(id);
        return comment;
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        _context.Comment.Add(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment> UpdateAsync(int id, UpdateCommentDTO updateCommentDTO)
    {
        var existing = await _context.Comment.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
            throw new InvalidOperationException("Comment not found");
        if (!string.IsNullOrWhiteSpace(updateCommentDTO.Content))
            existing.Content = updateCommentDTO.Content;
        if (!string.IsNullOrWhiteSpace(updateCommentDTO.Title))
            existing.Title = updateCommentDTO.Title;
        if (updateCommentDTO.StockId.HasValue)
            existing.StockId = updateCommentDTO.StockId.Value;
        await _context.SaveChangesAsync();
        return existing;
    }

    public Task<Comment> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


}
