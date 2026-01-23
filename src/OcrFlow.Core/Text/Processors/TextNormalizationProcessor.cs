using System.Text.RegularExpressions;

public class TextNormalizationProcessor : ITextProcessor
{
    public string Process(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        input = Regex.Replace(input, @"\s+([.,;:!?])", "$1");
        input = Regex.Replace(input, @"([.,;:!?])([A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź])", "$1 $2");
        input = Regex.Replace(input, @"([.,]){2,}", "$1");

        input = Regex.Replace(input, @"""{2,}", @"""");
        input = Regex.Replace(input, @"\s+""", @"""");
        input = Regex.Replace(input, @"""([A-Za-zĄĆĘŁŃÓŚŻŹąćęłńóśżź])", @""" $1");

        input = Regex.Replace(input, @"\s+\)", ")");
        input = Regex.Replace(input, @"\(\s+", "(");

        input = Regex.Replace(input, @"[_]{2,}", "_");
        input = Regex.Replace(input, @"[ \t]{2,}", " ");

        return input.Trim();
    }
}