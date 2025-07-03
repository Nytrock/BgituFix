using System;
using System.Text.RegularExpressions;

public static class StringUtils {
    public static string GenerateCopyName(this string input) {
        Regex regex = new(@"\((\d+)\)$");
        MatchCollection matches = regex.Matches(input);

        if (matches.Count == 0)
            return input + " (1)";

        string numberInBrackets = matches[0].Value;
        int number = Convert.ToInt32(numberInBrackets[1..^1]);
        return regex.Replace(input, $"({number + 1})");
    }

    public static string RemoveCopyName(this string input) {
        Regex regex = new(@" \((\d+)\)$");
        MatchCollection matches = regex.Matches(input);

        if (matches.Count == 0)
            return input;

        return regex.Replace(input, "");
    }
}
