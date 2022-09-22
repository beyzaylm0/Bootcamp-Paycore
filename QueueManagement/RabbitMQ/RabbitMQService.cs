using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using QueueManagement.ValueObjects;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace QueueManagement.RabbitMQ
{
    public class RabbitMQService
    {
        private readonly RabbitMQConfiguration _config;
        private readonly ILogger<RabbitMQService> _logger;
        private IConnection _connection;
        private IModel _channel;
        public RabbitMQService(IOptions<RabbitMQConfiguration> options, ILogger<RabbitMQService> logger)
        {
            _config = options.Value;
            _logger = logger;
        }
        //Connect rabbitMQ start
        public void Start()
        {
            try
            {
                //We define the host that RabbitMQ will connect to.
                //If we want to take any security measures,
                //it is sufficient to define the password steps from the Management screen and set the "UserName" and "Password" properties in the factory.

                var factory = new ConnectionFactory()
                {
                    HostName = _config.Host,
                    UserName = _config.UserName,
                    Password = _config.Password,
                    Port = _config.Port,
                    DispatchConsumersAsync = true
                };

                _connection = factory.CreateConnection();
                if (_connection != null)
                    _channel = _connection.CreateModel();
                _logger.LogInformation("RabbitMQ connection is succesfull");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }
        public void QueueDeclare(string queueName)
        {
            _channel.QueueDeclare(queueName,true,false,false);

        }
        public void SendMessage<T>(T message, string queueName)
        {
            if (_channel.IsOpen)
            {
                var messageStr = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(messageStr);
                _channel.BasicPublish("", queueName, null, bytes);
            }
        }


        public async Task RegisterConsumerAsync<T>(string queueName, Func<T, Task> function)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            //Received event will always be in listen mode
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();

                    var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));

                    await function(message);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception e)
                {

                    _channel.BasicNack(ea.DeliveryTag, false, true);
                    _logger.LogError(e.ToString());
                }



            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            await Task.CompletedTask;
        }
    }
}
