using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Infrastructure.Extensions
{
    public static class ContextExtensions
    {
        public static async Task<TEntity> FindByIdAsync<TEntity>(this IQueryable<TEntity> query, Guid id)
        where TEntity : class, IIdentifiableEntity
        {
            return await query.SingleOrDefaultAsync(entity => entity.Id == id);
        }
        
        public static async Task<TEntity> FindByNameAsync<TEntity>(this IQueryable<TEntity> query, string name)
            where TEntity : class, INamedEntity
        {
            return await query.SingleOrDefaultAsync(entity => entity.Name == name);
        }

    }
}