using System.Text.RegularExpressions;

namespace OcrFlow.Cli.Diagnostics
{
    public static class ExceptionSanitizer
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
            value = Regex.Replace(
                value,
                @"[A-Z]:\\Users\\[^\\]+\\",
                @"C:\Users\<user>\",
                RegexOptions.IgnoreCase);

            // Linux / macOS home
            value = Regex.Replace(
                value,
                @"/home/[^/]+/",
                "/home/<user>/",
                RegexOptions.IgnoreCase);

            // Bearer tokens
            value = Regex.Replace(
                value,
                @"Bearer\s+[A-Za-z0-9\-\._~\+\/]+=*",
                "Bearer <redacted>",
                RegexOptions.IgnoreCase);

            // Connection strings (very rough)
            value = Regex.Replace(
                value,
                @"(Password|Pwd)=([^;]+)",
                "$1=<redacted>",
                RegexOptions.IgnoreCase);

            // Trim length (URL safety)
            if (value.Length > MaxLength)
                value = value[..MaxLength] + "\n<trimmed>";

            return value.Trim();
        }
    }
}
