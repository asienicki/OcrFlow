using Spectre.Console;

namespace OcrFlow.Cli.Diagnostics
{
    public static class CrashReporter
    {
        private static int _reported; // 0/1 – tylko raz

        public static void Handle(Exception ex)
        {
            if (ex == null) return;
            if (Interlocked.Exchange(ref _reported, 1) == 1)
                return;

            try
            {
                AnsiConsole.WriteException(ex);

                var formatted = SpectreExceptionFormatter.Format(ex);
                var sanitized = ExceptionSanitizer.Sanitize(formatted);

                var hash = StackTraceHasher.Compute(sanitized);
                var trimmed = ExceptionTrimmer.TrimSmart(sanitized);

                GitHubIssueReporter.ReportWithPrompt(
                    $"[hash:{hash}]\n\n{trimmed}",
                    hash
                );
            }
            catch
            {
                // absolutny fallback: nie wysypuj crasha crashem
            }
        }
    }
}