using Microsoft.Extensions.Options;
using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Flow.Models.Options;
using SixLabors.ImageSharp.Processing;

namespace OcrFlow.Core.Flow.Steps
{
    public sealed class NormalizeDpiStep : IOcrStep
    {
        public bool IsEnabled { get; }

        private readonly int _targetDpi;

        public NormalizeDpiStep(IOptions<OcrFlowOptions> options)
        {
            IsEnabled = options.Value.NormalizeDpi.Enabled;
            _targetDpi = options.Value.NormalizeDpi.TargetDpi;
        }

        public ValueTask ExecuteAsync(OcrState state, CancellationToken ct)
        {
            if (!IsEnabled)
                return ValueTask.CompletedTask;

            if (state.Image is null)
                throw new InvalidOperationException("Image not loaded.");

            var currentDpi = state.Dpi ?? _targetDpi;

            if (currentDpi == _targetDpi)
                return ValueTask.CompletedTask;

            var scale = (double)_targetDpi / currentDpi;

            var newWidth = (int)Math.Round(state.Image.Width * scale);
            var newHeight = (int)Math.Round(state.Image.Height * scale);

            state.Image.Mutate(ctx =>
                ctx.Resize(newWidth, newHeight, KnownResamplers.Lanczos3));

            state.Dpi = _targetDpi;

            return ValueTask.CompletedTask;
        }
    }
}
