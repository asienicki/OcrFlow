namespace OcrFlow.Core
{
    public static class PathBuilder
    {
        public static string GetPdfPath(OcrRunOptions options, string outputSourceImagePath)
        {
            return Path.Combine(GetPdfFolderPath(options),
                $"{options.PdfFileNamePrefix}{Path.GetFileNameWithoutExtension(outputSourceImagePath)}.pdf"
            );
        }

        public static string GetPdfFolderPath(OcrRunOptions options)
        {
            var folder = Path.Combine(options.OutputDirectory, "pdf");

            Directory.CreateDirectory(folder);

            return folder;
        }
        public static string GetMdFolderPath(OcrRunOptions options)
        {
            var folder = Path.Combine(options.OutputDirectory, "md");

            Directory.CreateDirectory(folder);

            return folder;
        }

        public static string GetMarkdownPath(OcrRunOptions options, string outputSourceImagePath)
        {
            return Path.Combine(GetMdFolderPath(options),
                $"{options.MarkdownFileNamePrefix}{Path.GetFileNameWithoutExtension(outputSourceImagePath)}.md"
            );
        }
    }
}
