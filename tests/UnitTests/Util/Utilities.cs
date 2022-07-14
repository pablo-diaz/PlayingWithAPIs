using System;

namespace UnitTests.Util
{
    internal static class Utilities
    {
        public static string ReturnValueAccordingToPatternString(string valueOrPattern)
        {
            if (string.IsNullOrEmpty(valueOrPattern))
                return valueOrPattern;

            var isPatternPresent = valueOrPattern.StartsWith("++") && valueOrPattern.EndsWith("++");
            if (!isPatternPresent)
                return valueOrPattern;

            return GetFillString(valueOrPattern, '.');
        }

        private static string GetFillString(string text, char fillChar)
        {
            var charCount = 0;

            try
            {
                charCount = Convert.ToInt32(text.Replace("+", ""));
            }
            catch
            {
                return text;
            }

            var newValue = "";
            for (int i = 1; i <= charCount; i++)
                newValue += fillChar;

            return newValue;
        }
    }
}
