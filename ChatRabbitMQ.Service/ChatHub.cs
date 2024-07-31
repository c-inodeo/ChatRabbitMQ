using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRabbitMQ.Service
{
    
    public class ChatHub : Hub
    {
        private readonly ILogger<RabbitMQConsumerService> _logger;
        public ChatHub(ILogger<RabbitMQConsumerService> logger)
        {
            _logger = logger;
        }
        public async Task SendMessage(string user, string message) 
        {
            //_logger.LogInformation($"Received message from {user} : {message} \r\n");
            await Clients.All.SendAsync("ConsumeMessage",user, message);
            _logger.LogInformation($"Message broadcasted to SignalR clients - ChatHub Service \r\n");

        }
    }
}
