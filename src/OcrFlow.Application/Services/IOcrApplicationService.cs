using OcrFlow.Core.Flow.Models;

namespace OcrFlow.Application.Services;

public interface IOcrApplicationService
{
    Task<OcrOutput> ProcessAsync(
        OcrInput input,
        CancellationToken ct);
}