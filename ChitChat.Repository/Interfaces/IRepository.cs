using ChitChat.Data.Entities.Abstracts;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Repository.Interfaces
{
    public interface IRepository<T>
        where T : Entity
    {
        Task<List<T>> GetAllAsync(int page, int pageSize,string? search , bool sortDescending = true);
        Task<T> GetByIdAsync(ObjectId id);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> filter);
        Task AddAsync(T entity);
        Task UpdateAsync(ObjectId id, T entity);
        Task DeleteAsync(ObjectId id);
    }
}
