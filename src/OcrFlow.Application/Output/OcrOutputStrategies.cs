using OcrFlow.Core.Flow.Models;
using OcrFlow.Core.Output.Abstractions;

namespace OcrFlow.Application.Output
{
    public sealed class OcrOutputStrategies
    {
        private readonly IEnumerable<IOcrOutputStrategy> _strategies;

        public OcrOutputStrategies(IEnumerable<IOcrOutputStrategy> strategies)
            => _strategies = strategies;

        public async Task RunAsync(
            OcrOutput output,
            CancellationToken ct)
        {
            foreach (var strategy in _strategies)
            {
                if (strategy.ShouldRun())
                    await strategy.RunAsync(output, ct);
            }
        }
    }
}
