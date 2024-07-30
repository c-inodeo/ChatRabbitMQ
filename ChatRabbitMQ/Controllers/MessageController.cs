using ChatRabbitMQ.Service;
using Microsoft.AspNetCore.Mvc;

namespace ChatRabbitMQ.Controllers
{
    public class MessageController : Controller
    {
        private readonly RabbitMQService _rabbitmqService;
        public MessageController(RabbitMQService rabbitMQService)
        {
            _rabbitmqService = rabbitMQService;
        }
        [HttpPost]
        public IActionResult SendMessage([FromBody] string message) 
        {
            _rabbitmqService.SendMessage(message);
            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
