namespace DPSIW.Common.Agents
{
    public interface IAgent
    {
        Task ProcessAsync(CancellationToken token, string message);
    }
}
