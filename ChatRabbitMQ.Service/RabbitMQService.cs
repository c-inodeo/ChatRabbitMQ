using ChatRabbitMQ.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public void SendMessage(MessageModel message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(
                exchange:"",
                routingKey:"message_queue",
                basicProperties: null,
                body: body
            );
        }
        public void ConsumeMessage(Action<MessageModel> handleMessage) 
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            { 
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<MessageModel>(json);
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
