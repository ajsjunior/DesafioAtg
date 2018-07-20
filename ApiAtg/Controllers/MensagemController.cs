using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NucleoAtg.Dados;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApiAtg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensagemController : Controller
    {
        private const string AMQP_SERVIDOR = "35.199.98.99";
        private const int AMQP_PORTA = 5672;
        private const string AMQP_FILA = "fila_alcir";

        [HttpPost]
        public void Post(Mensagem mensagem)
        {
            var msgString = JsonConvert.SerializeObject(mensagem);
            EnviaMensagemAmqp(msgString);
            RecebeMensagemAmqp();
        }

        private void EnviaMensagemAmqp(string msgString)
        {
            var factory = new ConnectionFactory()
            {
                HostName = AMQP_SERVIDOR,
                Port = AMQP_PORTA
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: AMQP_FILA,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    // Transforma mensagem em JSON
                    var body = Encoding.UTF8.GetBytes(msgString);

                    //Seta a mensagem como persistente
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: AMQP_FILA,
                        basicProperties: properties,
                        body: body);
                }
            }
        }

        private void RecebeMensagemAmqp()
        {
            var factory = new ConnectionFactory()
            {
                HostName = AMQP_SERVIDOR,
                Port = AMQP_PORTA
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: AMQP_FILA,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.ConsumerTag = Guid.NewGuid().ToString(); // Tag de identificação do consumidor no RabbitMQ
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var msgBytes = Encoding.UTF8.GetString(body);
                        var retorno = JsonConvert.DeserializeObject<RetornoMensagem>(msgBytes);
                        if (!retorno.Status.Value)
                            throw new ApplicationException($"A mensagem #{retorno.Id} retornou {retorno.Msgs?.Length} erros.");
                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    channel.BasicConsume(
                        queue: AMQP_FILA,
                        autoAck: false,
                        consumer: consumer);
                }
            }
        }
    }
}