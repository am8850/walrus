namespace DPSIW.Common.Exceptions
{
    public class DeadLetterException : Exception
    {
        public DeadLetterException(string message) : base(message)
        { }
    }
}
