using System;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    Task<List<Comments>> GetAllsAsync();
    Task<Comments> GetByIdAsync(int id);
    Task<Comments> CreateAsync(Comments commentModel);
    Task<Comments> UpdateAsync(int id, Comments commentModel);
    Task<Comments> DeleteByIdAsync(int id);
}
