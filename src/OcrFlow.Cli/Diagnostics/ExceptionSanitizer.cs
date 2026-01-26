using System.Text.RegularExpressions;

namespace OcrFlow.Cli.Diagnostics;

public static partial class ExceptionSanitizer
{
    private const int MaxLength = 4000;

    public static string Sanitize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var value = input;

        // user name
        value = value.Replace(Environment.UserName, "<user>");

        // Windows paths: C:\Users\X\
        value = WindowsUserPathRegex().Replace(
            value,
            @"C:\Users\<user>\");

        // Linux / macOS home
        value = UnixHomeRegex().Replace(
            value,
            "/home/<user>/");

        // Bearer tokens
        value = BearerTokenRegex().Replace(
            value,
            "Bearer <redacted>");

        // Connection strings
        value = PasswordRegex().Replace(
            value,
            "$1=<redacted>");

        // Trim length (URL safety)
        if (value.Length > MaxLength)
            value = value[..MaxLength] + "\n<trimmed>";

        return value.Trim();
    }

    [GeneratedRegex(@"[A-Z]:\\Users\\[^\\]+\\", RegexOptions.IgnoreCase)]
    private static partial Regex WindowsUserPathRegex();

    [GeneratedRegex(@"/home/[^/]+/", RegexOptions.IgnoreCase)]
    private static partial Regex UnixHomeRegex();

    [GeneratedRegex(@"Bearer\s+[A-Za-z0-9\-\._~\+\/]+=*", RegexOptions.IgnoreCase)]
    private static partial Regex BearerTokenRegex();

    [GeneratedRegex(@"(Password|Pwd)=([^;]+)", RegexOptions.IgnoreCase)]
    private static partial Regex PasswordRegex();
}