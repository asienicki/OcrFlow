public sealed class OcrRunContext
{
    public OcrRunOptions Options { get; private set; } = null!;

    public void Initialize(OcrRunOptions options)
    {
        if (Options != null)
            throw new InvalidOperationException("Already initialized");

        Options = options;
    }
}