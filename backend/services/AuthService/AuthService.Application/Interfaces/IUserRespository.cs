using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces
{
    public interface IUserRespository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
