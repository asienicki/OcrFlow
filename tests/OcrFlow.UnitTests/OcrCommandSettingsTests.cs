using OcrFlow.Cli.Commands;

namespace OcrFlow.UnitTests;

public sealed class OcrCommandSettingsTests
{
    [Fact]
    public void Validate_WhenInputDirIsNull_ReturnsError()
    {
        var settings = new OcrCommandSettings
        {
            InputDirArg = null,
            InputDirOption = null
        };

        var result = settings.Validate();

        Assert.False(result.Successful);
        Assert.Contains("Input directory is required", result.Message);
    }

    [Fact]
    public void Validate_WhenInputDirDoesNotExist_ReturnsError()
    {
        var settings = new OcrCommandSettings
        {
            InputDirArg = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        };

        var result = settings.Validate();

        Assert.False(result.Successful);
        Assert.Contains("Directory does not exist", result.Message);
    }

    [Fact]
    public void Validate_WhenMarkdownOptionsConflict_ReturnsError()
    {
        using var dir = new TempDir();

        var settings = new OcrCommandSettings
        {
            InputDirArg = dir.Path,
            NoMarkdown = true,
            OnlyMarkdown = true
        };

        var result = settings.Validate();

        Assert.False(result.Successful);
        Assert.Contains("--nomarkdown", result.Message);
        Assert.Contains("--onlymarkdown", result.Message);
    }

    [Fact]
    public void Validate_WhenPdfOptionsConflict_ReturnsError()
    {
        using var dir = new TempDir();

        var settings = new OcrCommandSettings
        {
            InputDirArg = dir.Path,
            NoPdf = true,
            OnlyPdf = true
        };

        var result = settings.Validate();

        Assert.False(result.Successful);
        Assert.Contains("--nopdf", result.Message);
        Assert.Contains("--onlypdf", result.Message);
    }

    [Fact]
    public void Validate_WhenConfigurationIsValid_ReturnsSuccess()
    {
        using var dir = new TempDir();

        var settings = new OcrCommandSettings
        {
            InputDirArg = dir.Path
        };

        var result = settings.Validate();

        Assert.True(result.Successful);
    }

    [Fact]
    public void InputDir_WhenSourceOptionProvided_TakesPrecedence()
    {
        using var argDir = new TempDir();
        using var optDir = new TempDir();

        var settings = new OcrCommandSettings
        {
            InputDirArg = argDir.Path,
            InputDirOption = optDir.Path
        };

        Assert.Equal(optDir.Path, settings.InputDir);
    }

    [Fact]
    public void Lang_DefaultValue_IsEng()
    {
        var settings = new OcrCommandSettings();

        Assert.Equal("eng", settings.Lang);
    }

    /// <summary>
    /// Minimal helper to avoid touching real filesystem state.
    /// </summary>
    private sealed class TempDir : IDisposable
    {
        public string Path { get; } = Directory.CreateDirectory(
            System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString())
        ).FullName;

        public void Dispose()
        {
            if (Directory.Exists(Path))
                Directory.Delete(Path, true);
        }
    }
}
