using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using ChatRabbitMQ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRabbitMQ.Service
{
    public class ConsoleInputService : BackgroundService
    {
        private readonly RabbitMQService _rabbitmqService;
        private readonly IHubContext<ChatHub> _hubContext;
        public ConsoleInputService(RabbitMQService rabbitmqService, IHubContext<ChatHub> hubContext)
        {
            _rabbitmqService = rabbitmqService;
            _hubContext = hubContext;
        }
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () => 
            {
                while (!cancellationToken.IsCancellationRequested) 
                { 
                    var content = Console.ReadLine();
                    var message = new MessageModel("Console", content);
                    _rabbitmqService.SendMessage(message);
                    Console.Write($"Message sent: {message.Content} \r\n ");
                    await _hubContext.Clients.All.SendAsync("ConsumeMessage", message.Sender, message.Content);
                }
            });
            return Task.CompletedTask;
        }
    }
}
