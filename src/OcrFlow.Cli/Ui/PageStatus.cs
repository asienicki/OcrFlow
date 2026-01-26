namespace OcrFlow.Cli.Ui;

public sealed class PageStatus
{
    public int PageNo { get; init; }
    public string File { get; init; } = "";
    public string Status { get; set; } = "pending";
    public long OcrMs { get; set; }
}
