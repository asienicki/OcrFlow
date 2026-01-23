using Spectre.Console;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace OcrFlow.App.Diagnostics
{
    public static class GitHubIssueReporter
    {
        private const string Owner = "asienicki"; // TODO: set GitHub owner
        private const string Repo = "OcrFlow";   // TODO: set GitHub repo
        private const string IssueTemplate = null;//"crash.yml"; // optional

        public static void ReportWithPrompt(string body, string hash)
        {
            var confirm = AnsiConsole.Confirm(
                $"Application crashed (hash {hash}). Open GitHub issue?",
                defaultValue: false);

            if (!confirm)
                return;

            Report(body, hash);
        }

        public static void Report(string sanitizedException, string hash)
        {
            if (string.IsNullOrWhiteSpace(sanitizedException))
                return;

            var url = BuildIssueUrl(sanitizedException, hash);
            OpenBrowser(url);
        }

        private static string BuildIssueUrl(string bodyContent, string hash)
        {
            var title = Url($"Crash [{hash}]");

            var body = Url($@"### What happened
The application crashed.

### Stack trace
```
{bodyContent}
```
");

            var baseUrl = $"https://github.com/{Owner}/{Repo}/issues/new";

            var sb = new StringBuilder(baseUrl);
            sb.Append($"?title={title}");
            sb.Append($"&body={body}");
            sb.Append("&labels=bug,crash");

            if (!string.IsNullOrWhiteSpace(IssueTemplate))
                sb.Append($"&template={IssueTemplate}");

            return sb.ToString();
        }

        private static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    Process.Start("open", url);
                else
                    Process.Start("xdg-open", url);
            }
        }

        private static string Url(string value)
        {
            return WebUtility.UrlEncode(value);
        }
    }
}