using System.Runtime.InteropServices;
using Tesseract;

namespace OcrFlow.Core;

public static class TesseractEngineExtensions
{
    public static void DisableTesseractLogs(this TesseractEngine engine)
    {
        var nullDevice =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "nul"
                : "/dev/null";

        _ = engine.SetVariable("debug_file", nullDevice);
    }
}