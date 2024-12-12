using dotenv.net;

namespace DPSIW.Common.Services
{
    public class SettingsService
    {
        public string ServiceBusConnectionString { get; private set; }
        public string ServiceBusQueueName { get; private set; }
        public string OpenAIEndpoint { get; private set; } = "";
        public string OpenAIKey { get; private set; } = "";
        public string OpenAIChatModel { get; private set; } = "gpt-4o";
        public string speechKey { get; private set; } = "";
        public string speechRegion { get; private set; } = "";
        public string storageConnectionString { get; private set; } = "";

        public SettingsService()
        {
            DotEnv.Load();
            this.ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString") ?? "";
            this.ServiceBusQueueName = Environment.GetEnvironmentVariable("ServiceBusQueueName") ?? "";
            this.OpenAIEndpoint = Environment.GetEnvironmentVariable("OpenAIEndpoint") ?? "";
            this.OpenAIKey = Environment.GetEnvironmentVariable("OpenAIKey") ?? "";
            this.OpenAIChatModel = Environment.GetEnvironmentVariable("OpenAIChatModel") ?? "gpt-4o";
            this.speechKey = Environment.GetEnvironmentVariable("SpeechKey") ?? "";
            this.speechRegion = Environment.GetEnvironmentVariable("SpeechRegion") ?? "";
            this.storageConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString") ?? "";

            if (string.IsNullOrEmpty(this.ServiceBusConnectionString) || string.IsNullOrEmpty(this.ServiceBusQueueName))
            {
                //throw new Exception("ServiceBusConnectionString and ServiceBusQueueName must be set in the environment");
                Console.WriteLine("ServiceBusConnectionString and ServiceBusQueueName must be set in the environment");
                Environment.Exit(1);
            }
        }
    }
}
