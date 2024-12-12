using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System.Text;
using System.Text.Json;

namespace DPSIW.Common.Services
{
    public class AzureServiceBusService
    {
        private readonly ServiceBusClient client;
        private readonly ServiceBusAdministrationClient? adminClient;
        private readonly ServiceBusSender sender;

        public AzureServiceBusService(string connStr, string queueName)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            client = new ServiceBusClient(connStr, clientOptions);
            try
            {
                adminClient = new ServiceBusAdministrationClient(connStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to create admin client: {ex.Message}");
            }

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

        // Count queues in queue
        public async Task<long> CountMessages(string queueName)
        {
            if (adminClient is not null)
            {
                var props = await adminClient.GetQueueRuntimePropertiesAsync(queueName);
                return props.Value.ActiveMessageCount;
            }
            Console.WriteLine("Admin client was or could not be created");
            return -1;
        }

        // Count queues in queue
        public async Task<bool> PurgeQueue(string queueName)
        {
            if (client is not null)
            {
                ServiceBusReceiver receiver = client.CreateReceiver(queueName,
                    new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

                while ((await receiver.PeekMessageAsync()) != null)
                {
                    // receive in batches of 100 messages.
                    await receiver.ReceiveMessagesAsync(100);
                }
                return true;
            }            
            return false;
        }
    }
}
