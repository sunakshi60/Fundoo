using MessageService.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequestModel model)
        {
            Console.WriteLine("EMAIL: " + model.Email);
            Console.WriteLine("SUBJECT: " + model.Subject);
            Console.WriteLine("BODY: " + model.Body);

            await _messageService.SendEmailAsync(
                model.Email,
                model.Subject,
                model.Body
            );

            return Ok("Email sent successfully");
        }
    }
}
