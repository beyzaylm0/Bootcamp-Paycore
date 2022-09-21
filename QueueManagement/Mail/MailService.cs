using QueueManagement.RabbitMQ;
using QueueManagement.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace QueueManagement.Mail
{
    public class MailService : BackgroundService //Allows the email service to run in the background

    {
        // QueueName : GönderilecekMailler

        // Start => Rabbitmq connect, queue declare, consumer start

        // kuyrukla => rabbit'e gödnerilecek maili gönderecek

        // Consume eden metotta gelen mesajları email olarak gödnerecek.
        private readonly RabbitMQService _rabbitMQService;

        private const string QUEUE_NAME = "MailsToBeSent";
        private readonly SmtpSettings smtpSettings;
        private readonly ILogger<MailService> _logger;

        public MailService(RabbitMQService rabbitMQService, IOptionsMonitor<SmtpSettings> smtpSettings, ILogger<MailService> logger)
        {
            this.smtpSettings = smtpSettings.CurrentValue;
            _rabbitMQService = rabbitMQService;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _rabbitMQService.Start();
            _rabbitMQService.QueueDeclare(QUEUE_NAME);
            // Start => Rabbitmq connect, queue declare, consumer start

            await _rabbitMQService.RegisterConsumerAsync<MailModel>(QUEUE_NAME, async (message) => await MessageReceivedAsync(message));
        }

        private async Task MessageReceivedAsync(MailModel mailOBJ)
        {
           await SendMailAsync(mailOBJ);
            await Task.CompletedTask;

            // MAİL OLARAK GONDER
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(async () => { await StartAsync(); });

            return Task.CompletedTask;
        }

        private async Task SendMailAsync(MailModel mailOBJ)
        {
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = smtpSettings.Host,
                Port = smtpSettings.Port,
                EnableSsl = smtpSettings.EnableSsl,
                DeliveryFormat = SmtpDeliveryFormat.International,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                    {
                    UserName = smtpSettings.UserName,
                    Password = smtpSettings.Password,

                }
            };
            MailAddress fromMail = new MailAddress(smtpSettings.UserName);
            MailAddress toMail = new MailAddress(mailOBJ.Email);
            MailMessage message = new MailMessage()
            {
                From = fromMail,
                Subject = "Email",
                Body = mailOBJ.Message
            };
            message.To.Add(toMail);
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception e)
            {

                _logger.LogError(e.ToString());
            }
        }

        public void AddToMailQueue(MailModel mailOBJ)
        {

            _rabbitMQService.SendMessage(mailOBJ, QUEUE_NAME);
        }
    }
}
