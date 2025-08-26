using Domain.DTOs.User;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task<User> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            AccountId = createUserDto.AccountId,
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            PhoneNumber = createUserDto.PhoneNumber,
            UserRole = createUserDto.UserRole,
            PreferredNotificationMethod = createUserDto.PreferredNotificationMethod,
            EmailNotificationsEnabled = createUserDto.EmailNotificationsEnabled,
            SmsNotificationsEnabled = createUserDto.SmsNotificationsEnabled,
            NotificationOptOutDate = createUserDto.NotificationOptOutDate,
            Email = createUserDto.Email,
            AlternateEmail = createUserDto.AlternateEmail,
            AlternatePhoneNumber = createUserDto.AlternatePhoneNumber,
            EmergencyContactName = createUserDto.EmergencyContactName,
            EmergencyContactPhone = createUserDto.EmergencyContactPhone,
            Status = createUserDto.Status,
            PreferredLanguage = createUserDto.PreferredLanguage
        };

        return await _userRepository.AddAsync(user, cancellationToken);
    }

    public async Task<User?> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (existingUser == null)
        {
            return null;
        }

        existingUser.AccountId = updateUserDto.AccountId;
        existingUser.FirstName = updateUserDto.FirstName;
        existingUser.LastName = updateUserDto.LastName;
        existingUser.PhoneNumber = updateUserDto.PhoneNumber;
        existingUser.UserRole = updateUserDto.UserRole;
        existingUser.PreferredNotificationMethod = updateUserDto.PreferredNotificationMethod;
        existingUser.EmailNotificationsEnabled = updateUserDto.EmailNotificationsEnabled;
        existingUser.SmsNotificationsEnabled = updateUserDto.SmsNotificationsEnabled;
        existingUser.NotificationOptOutDate = updateUserDto.NotificationOptOutDate;
        existingUser.Email = updateUserDto.Email;
        existingUser.AlternateEmail = updateUserDto.AlternateEmail;
        existingUser.AlternatePhoneNumber = updateUserDto.AlternatePhoneNumber;
        existingUser.EmergencyContactName = updateUserDto.EmergencyContactName;
        existingUser.EmergencyContactPhone = updateUserDto.EmergencyContactPhone;
        existingUser.Status = updateUserDto.Status;
        existingUser.PreferredLanguage = updateUserDto.PreferredLanguage;
        existingUser.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(existingUser, cancellationToken);
        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(id, cancellationToken))
        {
            return false;
        }

        await _userRepository.DeleteAsync(id, cancellationToken);
        return true;
    }

    public async Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _userRepository.ExistsAsync(id, cancellationToken);
    }
}