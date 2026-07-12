using System.IdentityModel.Tokens.Jwt;
using Booking.API.Application.Authorization;
using Booking.API.Application.Auth;
using Booking.API.Infrastructure.ExceptionHandling;
using Microsoft.AspNetCore.Authorization;
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
        /// Crea una cuenta de usuario nueva con rol <c>Customer</c>. Solo un Admin puede dar de alta cuentas.
        /// </summary>
        /// <param name="request">Nombre completo, username, email y password de la cuenta a crear.</param>
        /// <param name="cancellationToken">Token de cancelación de la petición.</param>
        /// <returns>Los datos públicos del usuario creado (nunca el hash de la contraseña).</returns>
        /// <response code="200">Usuario creado correctamente.</response>
        /// <response code="401">Falta el token o no es válido.</response>
        /// <response code="403">El usuario está logueado pero no es Admin.</response>
        /// <response code="409">Ya existe un usuario con ese username o email (<c>USER_ALREADY_EXISTS</c>).</response>
        [HttpPost("register")]
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Devuelve los datos del usuario autenticado, tomados directamente del token. Requiere estar logueado (cualquier rol).
        /// </summary>
        /// <response code="200">Token válido, datos del usuario actual.</response>
        /// <response code="401">Falta el token o no es válido.</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Me() => Ok(new
        {
            Id = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
            Username = User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value,
            Email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value,
            Role = User.FindFirst("role")?.Value
        });
    }
}
