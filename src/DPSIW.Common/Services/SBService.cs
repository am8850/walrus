using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

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
            try
            {
                // serialize ojbect to json
                var jsonData = JsonSerializer.Serialize(message);
                var sbMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonData));
                await sender.SendMessageAsync(sbMessage);
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Unable to send message: {ex.Message}");
            }
        }
    }
}
