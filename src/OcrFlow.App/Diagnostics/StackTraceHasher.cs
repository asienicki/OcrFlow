using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OcrFlow.App.Diagnostics
{
    public static class StackTraceHasher
    {
        public static string Compute(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "no-stack";

            var normalized = Normalize(input);

            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(normalized));

            return Convert.ToHexString(hash)[..12];
        }

        private static string Normalize(string value)
        {
            var v = value.ToLowerInvariant();

            // usuń ścieżki Windows
            v = Regex.Replace(v, @"[a-z]:\\[^:\n]+", "<path>");

            // usuń ścieżki unix
            v = Regex.Replace(v, @"/[^ \n]+/", "<path>/");

            // usuń numery linii
            v = Regex.Replace(v, @"line\s+\d+", "");

            // zbij whitespace
            v = Regex.Replace(v, @"\s+", "");

            return v;
        }
    }
}