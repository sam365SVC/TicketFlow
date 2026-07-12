using Booking.API.Application.Abstractions;
using Booking.API.Application.Exceptions;
using Booking.Domain;

namespace Booking.API.Application.Auth;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator)
{
    public async Task<User> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingByUsername = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        var existingByEmail = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingByUsername is not null || existingByEmail is not null)
        {
            throw new UserAlreadyExistsException();
        }

        var user = new User
        {
            FullName = request.FullName,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = UserRole.Customer,
            Active = true,
            RequiresPwdChange = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (user is null || !user.Active || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        var (token, expiresAt) = jwtTokenGenerator.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}
