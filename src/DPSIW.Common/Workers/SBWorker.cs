using Azure.Identity;
using Azure.Messaging.ServiceBus;
using DPSIW.Common.Agents;
using DPSIW.Common.Exceptions;
using DPSIW.Common.Models;
using System.Text.Json;

namespace DPSIW.Common.Workers
{
    public class SBWorker(string connStr, string queueName) : IWorker
    {
        public async Task ProcessAsync(CancellationToken token, int instances)
        {
            // the client that owns the connection and can be used to create senders and receivers
            ServiceBusClient client;

            // the processor that reads and processes messages from the queue
            ServiceBusProcessor processor;

            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            // TODO: Replace the connection string DefaultAzureClient credentials
            client = new ServiceBusClient(
                connStr,
                clientOptions);
            //new DefaultAzureCredential(),
            //clientOptions);

            ServiceBusProcessor[] processors = [];
            try
            {
                Console.WriteLine($"Starting {instances} workers for: {queueName}");
                for (var i = 0; i < instances; i++)
                {
                    processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
                    // add handler to process messages
                    processor.ProcessMessageAsync += MessageHandler;
                    // add handler to process any errors
                    processor.ProcessErrorAsync += ErrorHandler;
                    // Append processor to list
                    processors.Append(processor);
                    // start processing
                    await processor.StartProcessingAsync();
                }

                // Wait indefinetely
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(500);
                }

                // stop processing 
                Console.WriteLine("\nStopping the receiver(s)...");
                //await processor.StopProcessingAsync();
                foreach (var p in processors)
                {
                    await p.StopProcessingAsync();
                }
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                //await processor.DisposeAsync();
                //await client.DisposeAsync();
                foreach (var item in processors)
                {
                    await item.DisposeAsync();
                }
                await client.DisposeAsync();
            }

        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                // Body is json string. Deserialize it to Message object
                string body = args.Message.Body.ToString();
                var message = JsonSerializer.Deserialize<Message>(body)!;
                IAgent? agent = null;

                if (message.type.Equals("medicalnotesagent", StringComparison.InvariantCultureIgnoreCase))
                {
                    agent = new MedicalNotesAgent();
                    await agent.ProcessAsync(args.CancellationToken, body);
                }
                else
                {
                    Console.WriteLine("Unknown message type");
                }

                // complete the message. message is deleted from the queue. 
                await args.CompleteMessageAsync(args.Message);
            }
            catch(DeadLetterException ex)
            {
                Console.WriteLine($"Error processing the message: {ex.Message}");
                await args.DeadLetterMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing the message: {ex.Message}");
                await args.AbandonMessageAsync(args.Message);
            }
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
