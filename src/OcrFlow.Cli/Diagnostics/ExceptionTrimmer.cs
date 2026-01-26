using System.Text;

namespace OcrFlow.Cli.Diagnostics
{
    public static class ExceptionTrimmer
    {
        public static string TrimSmart(string input, int maxLength = 3500)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            if (input.Length <= maxLength)
                return input;

            var head = input[..(maxLength / 2)];
            var tail = input[^(maxLength / 2)..];

            var sb = new StringBuilder();
            _ = sb.AppendLine(head.TrimEnd());
            _ = sb.AppendLine();
            _ = sb.AppendLine("<trimmed>");
            _ = sb.AppendLine();
            _ = sb.AppendLine(tail.TrimStart());

            return sb.ToString();
        }
    }
}