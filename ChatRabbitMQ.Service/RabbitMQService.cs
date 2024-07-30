using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRabbitMQ.Service
{
    public class RabbitMQService
    {
        private readonly IConnection _conn;
        private readonly IModel _channel;

        public RabbitMQService()
        {
           var factory = new ConnectionFactory() { HostName = "localhost"};
            _conn = factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(
                queue: "message_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }
        public void SendMessage([FromBody] string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange:"",
                routingKey:"message_queue",
                basicProperties: null,
                body: body
            );
        }
        public void ConsumeMessage(Action<string> handleMessage) 
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            { 
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handleMessage(message);
            };
            _channel.BasicConsume(
                queue: "message_queue",
                autoAck: true,
                consumer: consumer
            );
        }
    }
}
