using OcrFlow.Application.Output;
using OcrFlow.Core.Flow;
using OcrFlow.Core.Flow.Abstractions;
using OcrFlow.Core.Flow.Models;

namespace OcrFlow.Application.Services
{
    public sealed class OcrApplicationService : IOcrApplicationService
    {
        private readonly IOcrStepFactory _stepFactory;
        private readonly OcrOutputStrategies _outputStrategies;

        public OcrApplicationService(
            IOcrStepFactory stepFactory,
            OcrOutputStrategies outputStrategies)
        {
            _stepFactory = stepFactory;
            _outputStrategies = outputStrategies;
        }

        public async Task<OcrOutput> ProcessAsync(
            OcrInput input,
            CancellationToken ct)
        {
            var state = new OcrState(input);

            var process = OcrProcessFlow
                .Start(_stepFactory)
                .LoadImage()
                .Rotate()
                .Deskew()
                .NormalizeDpi()
                .TesseractOcr()
                .Build();

            await process.ExecuteAsync(state, ct);

            if (state.Output is null)
                throw new InvalidOperationException("OcrOutput is null");

            await _outputStrategies.RunAsync(state.Output, ct);

            return state.Output;
        }
    }
}
