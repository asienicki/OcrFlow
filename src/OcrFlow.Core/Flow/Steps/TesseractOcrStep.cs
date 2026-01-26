using Microsoft.Extensions.Options;
using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Flow.Models.Options;
using SixLabors.ImageSharp;
using Tesseract;

namespace OcrFlow.Core.Flow.Steps
{
    public sealed class TesseractOcrStep : IOcrStep
    {
        public bool IsEnabled => true;

        private readonly TesseractOptions _options;

        public TesseractOcrStep(IOptions<OcrFlowOptions> options)
            => _options = options.Value.Tesseract;

        public ValueTask ExecuteAsync(OcrState state, CancellationToken ct)
        {
            if (state.Image is null)
                throw new InvalidOperationException("Image not loaded.");

            // wybór tessdata (na razie pierwszy – później można rozszerzyć)
            var source = _options.DataSources.First();

            using var engine = new TesseractEngine(
                source.Path,
                string.Join("+", source.Languages),
                EngineMode.LstmOnly);

            engine.DisableTesseractLogs();

            using var pix = ConvertToPix(state.Image);
            using var page = engine.Process(pix, PageSegMode.Auto);

            state.Output = new OcrOutput
            {
                SourceImagePath = state.Input.ImagePath,
                HocrText = page.GetHOCRText(0),
                RawText = page.GetText() // TYLKO jeśli naprawdę potrzebujesz
            };

            return ValueTask.CompletedTask;
        }

        // ImageSharp.Image -> Pix
        private static Pix ConvertToPix(Image image)
        {
            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            ms.Position = 0;
            return Pix.LoadFromMemory(ms.ToArray());
        }
    }
}
