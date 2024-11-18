using DPSIW.Common.Services;
using DPSIW.Common.Workers;


namespace DPSIW.CLI
{
    internal partial class Program
    {
        static async Task<int> Main(string[] args)
        {
            // DI
            var services = CreateServices();
            

            // Create the root command
            var rootCommand = new RootCommand("DPSiw for .NET");
            rootCommand.SetHandler((context) =>
            {
                var token = context.GetCancellationToken();
                AnsiConsole.Markup("[green]DPSiw[/] .NET\n");
            });

            // Produce command
            var numberOption = new Option<int>(
            name: "--number",
            description: "Number of messages",
            getDefaultValue: () => 1);

            var produceCommand = new Command("produce", "Produce a moock message")
            {
                numberOption
            };

            produceCommand.SetHandler((number) =>
            {
                Console.WriteLine($"Producing messages: {number}");

            },numberOption);

            // Consume command
            var instancesOption = new Option<int>(
                name: "--instances",
                description: "Instances to start",
                getDefaultValue: () => 1);

            var consumeCommand = new Command("consume", "Consume messages");
            consumeCommand.AddOption(instancesOption);


            consumeCommand.SetHandler(async (context) =>
            {
                var processor = services.GetRequiredService<IProcessor>();
                int instances = context.ParseResult.GetValueForOption(instancesOption);
                var token = context.GetCancellationToken();
                await processor.ProcessAsync(token,instances);
            });

            // qclear command
            var qclearCommand = new Command("qclear", "Clear the queue");
            qclearCommand.SetHandler((context) =>
            {                
                
                Console.WriteLine("Clearing the queue");
            });

            // qclear command
            var qcountCommand = new Command("qcount", "Count the messages in the queue");
            qclearCommand.SetHandler((context) =>
            {

                Console.WriteLine("Counting the messages in the queue");
            });

            // Add commands to the root command
            rootCommand.AddCommand(produceCommand);
            rootCommand.AddCommand(consumeCommand);
            rootCommand.AddCommand(qclearCommand);
            rootCommand.AddCommand(qcountCommand);

            // Execute the CLI Command
            return await rootCommand.InvokeAsync(args);
            //return await consumeCommand.InvokeAsync(args);
        }

        private static ServiceProvider CreateServices()
        {
            Settings settings = new();
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IProcessor>(new SBWorker(settings.ServiceBusConnectionString,settings.ServiceBusQueueName))
            .AddSingleton<Settings>(settings)
            .BuildServiceProvider();

            return serviceProvider;
        }

    }
}
