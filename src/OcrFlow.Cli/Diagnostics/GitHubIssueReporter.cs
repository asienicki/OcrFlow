using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console;

namespace OcrFlow.Cli.Diagnostics;

public static class GitHubIssueReporter
{
    private const string Owner = "asienicki";
    private const string Repo = "OcrFlow";
    private const string IssueTemplate = "crash.yml";

    public static void ReportWithPrompt(string body, string hash)
    {
        var confirm = AnsiConsole.Confirm(
            $"Application crashed (hash {hash}). Open GitHub issue?",
            false);

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

    private static string BuildIssueUrl(string stacktrace, string hash)
    {
        var title = Url($"Crash [{hash}]");

        var baseUrl = $"https://github.com/{Owner}/{Repo}/issues/new";

        var sb = new StringBuilder(baseUrl);
        _ = sb.Append($"?title={title}");
        _ = sb.Append($"&stacktrace={Uri.EscapeDataString(stacktrace)}");
        _ = sb.Append("&what-happened=The application crashed.");
        _ = sb.Append($"&os={RuntimeInformation.OSDescription}");
        _ = sb.Append("&labels=bug,crash");

        if (!string.IsNullOrWhiteSpace(IssueTemplate))
            _ = sb.Append($"&template={IssueTemplate}");

        return sb.ToString();
    }

    private static void OpenBrowser(string url)
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch
        {
            _ = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true })
                : RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    ? Process.Start("open", url)
                    : Process.Start("xdg-open", url);
        }
    }

    private static string Url(string value)
    {
        return WebUtility.UrlEncode(value);
    }
}