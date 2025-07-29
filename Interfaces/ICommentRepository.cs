using System;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllsAsync();
    Task<Comment> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment commentModel);
    Task<Comment> UpdateAsync(int id, Comment commentModel);
    Task<Comment> DeleteByIdAsync(int id);
}
