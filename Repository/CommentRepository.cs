using System;
using api.ApplicationDbContext;
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

    public Task<Comment> UpdateAsync(int id, Comment commentModel)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
