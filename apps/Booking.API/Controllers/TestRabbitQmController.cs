using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using TicketFlow.Shared.Events;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestRabbitQmController(IBus bus) : Controller
    {
        [HttpPost("enviar")]
        public async Task<IActionResult> EnviarPrueba([FromBody] ReservationConfirmedEvent evento)
        {
            await bus.Send(evento);
            return Ok("¡Mensaje enviado a RabbitMQ exitosamente desde el controlador!");
        }
    }
}
