using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace OcrFlow.Core.Flow.Steps
{
    public sealed class LoadImageStep : IOcrStep
    {
        public bool IsEnabled => true;

        public async ValueTask ExecuteAsync(OcrState state, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(state);

            if (!File.Exists(state.Input.ImagePath))
                throw new FileNotFoundException(
                    "Image file not found",
                    state.Input.ImagePath);

            state.Image = await Image.LoadAsync(state.Input.ImagePath, ct);

            // DPI detection (fallback 300)
            var exif = state.Image.Metadata.ExifProfile;

            if (exif is not null &&
                exif.TryGetValue(ExifTag.XResolution, out var xRes) &&
                xRes?.Value is SixLabors.ImageSharp.Rational r &&
                r.Denominator != 0)
            {
                state.Dpi = (int)Math.Round((double)r.Numerator / r.Denominator);
            }
            else
            {
                state.Dpi = 300;
            }
        }
    }
}
