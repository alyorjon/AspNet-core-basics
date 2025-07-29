using System;
using api.ApplicationDbContext;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository:ICommentRepository
{
    private readonly ApplicationDBContext _context;
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Comments>> GetAllsAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public Task<Comments> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Comments> CreateAsync(Comments commentModel)
    {
        throw new NotImplementedException();
    }

    public Task<Comments> UpdateAsync(int id, Comments commentModel)
    {
        throw new NotImplementedException();
    }

    public Task<Comments> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
