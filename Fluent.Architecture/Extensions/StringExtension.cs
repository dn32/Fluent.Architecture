using System.Threading;

namespace Fluent.Architecture.Extensions
{
    public static class StringExtension
    {
        public static string TitleCase(this string text)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLowerInvariant());
        }

        public static string GetGlobalizationOfResourceWithParameters(this string text, params string[] parameters)
        {           
            return GetGlobalizationOfResource(text, text, parameters);
        }

        public static string GetGlobalizationOfResource(this string text, string defaultMessage = "", params string[] parameters)
        {
            if (Setup.GlobalizationService == null)
            {
                if (string.IsNullOrWhiteSpace(defaultMessage))
                {
                    return text;
                }
                else
                {
                    return defaultMessage;
                }
            }

            return Setup.GlobalizationService.GetResource(text, defaultMessage, parameters);
        }
    }
}
