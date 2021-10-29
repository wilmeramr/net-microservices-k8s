using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatFormService.AsyncDataServices;
using PlatFormService.Dtos;
using RabbitMQ.Client;

namespace PlatFormService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() { HostName = _configuration["RanbitMQHost"], Port = int.Parse(_configuration["RanbitMQPort"]) };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {

                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMq Connection Open, Sending message..");

                SendMessage(message);


            }
            else
            {

                Console.WriteLine("--> RabbitMq Connections Closed, not Sending..");

            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger"
                , routingKey: ""
                , basicProperties: null
                , body: body);

            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");

            if (_channel.IsOpen)
            {
                _channel.Dispose();
                _connection.Dispose();
            }

        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {

            Console.WriteLine("--> RabbitMQ Connection Shutdown");

        }
    }
}