using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.API.Models.Entities;

namespace UserManager.API.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<List<T>> GetAllUsers();
        public Task<T> GetUser(Guid id);
        public Task<T> GetUserByAttribute(string name, string value);
        public Task<T> AddUser(T entity);
        public Task<T> UpdateUser(T entity);
        public Task<T> DeleteUser(T entity);
    }
}
