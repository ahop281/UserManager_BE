using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.API.Data;
using UserManager.API.Models.Entities;

namespace UserManager.API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(UserDbContext userDbContext)
        {
            _context = userDbContext;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllUsers()
        {
            return await _dbSet.FromSqlRaw("SELECT * FROM[UserManagerDb].[dbo].[Users] ORDER BY CreatedAt DESC").ToListAsync();
            //return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetUser(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> AddUser(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateUser(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteUser(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
