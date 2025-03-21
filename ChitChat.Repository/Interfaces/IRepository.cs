﻿using ChitChat.Data.Entities.Abstracts;
using ChitChat.Repository.Helpers;
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
        Task<PagedList<T>> GetAllAsync(ItemQueryParams queryParams, Expression<Func<T, bool>> filterExpression = null);
        Task<T> GetByIdAsync(string id);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> filter);
        Task AddAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
        Task DeleteAsync(Expression<Func<T, bool>> filter);
    }
}
