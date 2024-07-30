using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRabbitMQ.Service
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly RabbitMQService _rabbitMQService;
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly ILogger<RabbitMQConsumerService> _logger;

        public RabbitMQConsumerService(
            RabbitMQService rabbitMQService, 
            IHubContext<ChatHub> chatHub, //SignalR
            ILogger<RabbitMQConsumerService> logger)
        {
            _rabbitMQService = rabbitMQService;
            _chatHub = chatHub;
            _logger = logger;
        }
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _rabbitMQService.ConsumeMessage(async message =>
            {
                _logger.LogInformation($"Received message from RabbitMQ: {message} \r\n");
                await _chatHub.Clients.All.SendAsync("ConsumeMessage", "RabbitMQ", message);
                _logger.LogInformation($"Message sent to SignalR clients - Consumer Service \r\n");

            });
            return Task.CompletedTask;
        }
    }
}
