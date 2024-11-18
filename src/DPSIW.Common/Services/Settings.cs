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
        public Settings()
        {
            DotEnv.Load();
            this.ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString") ?? "";
            this.ServiceBusQueueName = Environment.GetEnvironmentVariable("ServiceBusQueueName") ?? "";
        }
    }
}
