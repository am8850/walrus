using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotenv.net;

namespace DPSIW.Common.Services
{
    public class Settings
    {
        public string ServiceBusConnectionString { get; private set; }
        public string ServiceBusQueueName { get; private set; }
        public string OpenAIEndpoint { get; private set; } = "";
        public string OpenAIKey { get; private set; } = "";
        public string OpenAIChatModel { get; private set; } = "gpt-4o";
        public string speechKey { get; private set; } = "";
        public string speechRegion { get; private set; } = "";

        public Settings()
        {
            DotEnv.Load();
            this.ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString") ?? "";
            this.ServiceBusQueueName = Environment.GetEnvironmentVariable("ServiceBusQueueName") ?? "";
            this.OpenAIEndpoint = Environment.GetEnvironmentVariable("OpenAIEndpoint") ?? "";
            this.OpenAIKey = Environment.GetEnvironmentVariable("OpenAIKey") ?? "";
            this.OpenAIChatModel = Environment.GetEnvironmentVariable("OpenAIChatModel") ?? "gpt-4o";
            this.speechKey = Environment.GetEnvironmentVariable("SpeechKey") ?? "";
            this.speechRegion = Environment.GetEnvironmentVariable("SpeechRegion") ?? "";
        }
    }
}
