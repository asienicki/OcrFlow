using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OcrFlow.Cli.Diagnostics;

public static partial class StackTraceHasher
{
    public static string Compute(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "no-stack";

        var normalized = Normalize(input);

        var hash = SHA256.HashData(
            Encoding.UTF8.GetBytes(normalized));

        return Convert.ToHexString(hash)[..12];
    }

    private static string Normalize(string value)
    {
        var v = value.ToLowerInvariant();

        // usuń ścieżki Windows
        v = WindowsPathRegex().Replace(v, "<path>");

        // usuń ścieżki unix
        v = UnixPathRegex().Replace(v, "<path>/");

        // usuń numery linii
        v = LineNumberRegex().Replace(v, "");

        // zbij whitespace
        v = WhitespaceRegex().Replace(v, "");

        return v;
    }

    [GeneratedRegex(@"[a-z]:\\[^:\n]+", RegexOptions.IgnoreCase)]
    private static partial Regex WindowsPathRegex();

    [GeneratedRegex(@"/[^ \n]+/", RegexOptions.IgnoreCase)]
    private static partial Regex UnixPathRegex();

    [GeneratedRegex(@"line\s+\d+", RegexOptions.IgnoreCase)]
    private static partial Regex LineNumberRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}