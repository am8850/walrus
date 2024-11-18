using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPSIW.Common.Services
{
    public class MockProducer
    {
        public MockProducer()
        {
            
        }

        public async Task Produce()
        {
            await Task.CompletedTask;
        }
    }
}
