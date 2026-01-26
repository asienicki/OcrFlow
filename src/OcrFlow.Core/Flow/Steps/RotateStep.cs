using Microsoft.Extensions.Options;
using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Flow.Models.Options;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;

namespace OcrFlow.Core.Flow.Steps;

public sealed class RotateStep : IOcrStep
{
    public RotateStep(IOptions<OcrFlowOptions> options)
    {
        IsEnabled = options.Value.Rotate.Enabled;
    }

    public bool IsEnabled { get; }

    public ValueTask ExecuteAsync(OcrState state, CancellationToken ct)
    {
        if (!IsEnabled)
            return ValueTask.CompletedTask;

        if (state.Image is null)
            throw new InvalidOperationException("Image not loaded.");

        var exif = state.Image.Metadata.ExifProfile;

        if (exif is null ||
            !exif.TryGetValue(ExifTag.Orientation, out var orientationValue) ||
            orientationValue?.Value is not ushort orientation)
            return ValueTask.CompletedTask;

        state.Image.Mutate(ctx =>
        {
            switch (orientation)
            {
                case 3:
                    _ = ctx.Rotate(180);
                    break;
                case 6:
                    _ = ctx.Rotate(90);
                    break;
                case 8:
                    _ = ctx.Rotate(270);
                    break;
            }
        });

        // Reset orientation to normal
        exif.SetValue(ExifTag.Orientation, (ushort)1);

        return ValueTask.CompletedTask;
    }
}