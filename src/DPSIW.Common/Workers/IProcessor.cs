namespace DPSIW.Common.Workers
{
    public interface IProcessor
    {
        Task ProcessAsync(CancellationToken token, int instances);
    }
}
