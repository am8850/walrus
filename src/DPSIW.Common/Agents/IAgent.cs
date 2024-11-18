using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPSIW.Common.Agents
{
    public interface IAgent
    {
        Task ProcessAsync(CancellationToken token, string message);
    }
}
