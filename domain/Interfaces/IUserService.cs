using Domain.DTOs.User;
using Domain.Models;

namespace Domain.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<User> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<User?> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default);
}