using System;
using api.ApplicationDbContext;
using api.Interfaces;
using api.Models;

namespace api.Repository;

public class CommentRepository:ICommentRepository
{
    private readonly ApplicationDBContext _context;
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public Task<List<Comment>> GetAllsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Comment> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> CreateAsync(Comment commentModel)
    {
        throw new NotImplementedException();
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
