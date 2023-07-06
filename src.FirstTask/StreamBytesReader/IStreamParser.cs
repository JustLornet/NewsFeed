namespace StreamBytesReader
{
    public interface IStreamParser
    {
        string StreamMessage { get; }

        IEnumerable<string> SplittedStreamMessage { get; }
    }
}