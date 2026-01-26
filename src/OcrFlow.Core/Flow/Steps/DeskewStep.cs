using Microsoft.Extensions.Options;
using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Flow.Models.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OcrFlow.Core.Flow.Steps;

public sealed class DeskewStep : IOcrStep
{
    public bool IsEnabled { get; }

    public DeskewStep(IOptions<OcrFlowOptions> options)
        => IsEnabled = options.Value.Deskew.Enabled;

    public ValueTask ExecuteAsync(OcrState state, CancellationToken ct)
    {
        if (!IsEnabled)
            return ValueTask.CompletedTask;

        if (state.Image is null)
            throw new InvalidOperationException("Image not loaded.");

        using var img = state.Image.CloneAs<Rgba32>();

        using var preview = CreatePreview(img);
        var angle = DetectSkewAngle(preview);

        if (Math.Abs(angle) < 0.5)
            return ValueTask.CompletedTask;

        img.Mutate(ctx => ctx.Rotate((float)-angle));
        state.Image = img.Clone();

        return ValueTask.CompletedTask;
    }

    // ---------- helpers ----------

    private static Image<Rgba32> CreatePreview(Image<Rgba32> source)
    {
        const int maxWidth = 600;

        if (source.Width <= maxWidth)
            return source.Clone();

        var scale = (double)maxWidth / source.Width;
        var height = (int)Math.Round(source.Height * scale);

        return source.Clone(ctx => ctx.Resize(maxWidth, height));
    }

    private static double DetectSkewAngle(Image<Rgba32> image)
    {
        double bestAngle = 0;
        double bestScore = double.MinValue;

        // realny zakres dla skanów
        for (double angle = -3.0; angle <= 3.0; angle += 0.25)
        {
            using var rotated = image.Clone(ctx => ctx
                .Grayscale()
                .Rotate((float)angle)
                .BinaryThreshold(0.6f));

            var score = HorizontalProjectionVariance(rotated);
            if (score > bestScore)
            {
                bestScore = score;
                bestAngle = angle;
            }
        }

        return bestAngle;
    }

    private static double HorizontalProjectionVariance(Image<Rgba32> img)
    {
        var sums = new double[img.Height];

        img.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                var row = accessor.GetRowSpan(y);
                double sum = 0;

                for (int x = 0; x < row.Length; x++)
                    sum += row[x].R; // grayscale → R=G=B

                sums[y] = sum;
            }
        });

        double avg = sums.Average();
        double variance = 0;

        for (int i = 0; i < sums.Length; i++)
        {
            var d = sums[i] - avg;
            variance += d * d;
        }

        return variance;
    }
}
