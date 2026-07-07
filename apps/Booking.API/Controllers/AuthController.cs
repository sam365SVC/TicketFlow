using Booking.API.Application.Auth;
using Booking.API.Infrastructure.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    /// <summary>
    /// Registro e inicio de sesión de usuarios. Emite los tokens JWT que el resto de la API exige vía <c>Authorize</c>.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController(AuthService authService) : ControllerBase
    {
        /// <summary>
        /// Crea una cuenta de usuario nueva con rol <c>Customer</c>.
        /// </summary>
        /// <param name="request">Nombre completo, username, email y password de la cuenta a crear.</param>
        /// <param name="cancellationToken">Token de cancelación de la petición.</param>
        /// <returns>Los datos públicos del usuario creado (nunca el hash de la contraseña).</returns>
        /// <response code="200">Usuario creado correctamente.</response>
        /// <response code="409">Ya existe un usuario con ese username o email (<c>USER_ALREADY_EXISTS</c>).</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            var user = await authService.RegisterAsync(request, cancellationToken);

            return Ok(new RegisterResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }

        /// <summary>
        /// Autentica a un usuario existente y emite un token JWT.
        /// </summary>
        /// <param name="request">Username y password de la cuenta.</param>
        /// <param name="cancellationToken">Token de cancelación de la petición.</param>
        /// <returns>El token JWT y su fecha de expiración.</returns>
        /// <response code="200">Login correcto, token emitido.</response>
        /// <response code="401">Username o password inválidos (<c>INVALID_CREDENTIALS</c>).</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await authService.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}
