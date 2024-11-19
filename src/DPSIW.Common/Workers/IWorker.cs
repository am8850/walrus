namespace DPSIW.Common.Workers
{
    public interface IWorker
    {
        Task ProcessAsync(CancellationToken token, int instances);
    }
}
