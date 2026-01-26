namespace OcrFlow.Cli.Ui;

public interface IProgressReporter
{
    void PageStarted(int pageNo, string file);
    void PageCompleted(int pageNo, long ocrMs);
}