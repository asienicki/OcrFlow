using OcrFlow.Core.Flow.Steps.Execution;

namespace OcrFlow.Core.Flow.Abstractions;

public interface IOcrProcessFlow
{
    IOcrProcessFlow LoadImage();

    IOcrProcessFlow Rotate();

    IOcrProcessFlow Deskew();

    IOcrProcessFlow NormalizeDpi();

    IOcrProcessFlow TesseractOcr();

    OcrProcess Build();
}