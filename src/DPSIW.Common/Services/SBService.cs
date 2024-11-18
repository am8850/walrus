using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DPSIW.Common.Services
{
    public class SBService
    {
        private readonly ServiceBusClient client;
        private readonly ServiceBusSender sender;

        public SBService(string connStr, string queueName)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            client = new ServiceBusClient(connStr, clientOptions);
            sender = client.CreateSender(queueName);
        }

        public async Task SendMessage(object message)
        {
            // serialize ojbect to json
            var jsonData = JsonSerializer.Serialize(message);
            var sbMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonData));
            await sender.SendMessageAsync(sbMessage);
        }
    }
}
