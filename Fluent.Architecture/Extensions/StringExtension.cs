using System.Linq;
using System.Threading;

namespace Fluent.Architecture.Extensions
{
    public static class StringExtension
    {
        public static string TitleCase(this string text)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLowerInvariant());
        }

        //public static string GetGlobalizationOfResourceWithParameters(this string text, params string[] parameters)
        //{
        //    return GetGlobalizationOfResource(text, text, parameters);
        //}

        //public static string GetGlobalizationOfResource(this string text, string defaultMessage = "", params string[] parameters)
        //{
        //    if (Setup.GlobalizationService == null)
        //    {
        //        if (string.IsNullOrWhiteSpace(defaultMessage))
        //        {
        //            return text;
        //        }
        //        else
        //        {
        //            return defaultMessage;
        //        }
        //    }

        //    return Setup.GlobalizationService.GetResource(text, defaultMessage, parameters);
        //}

        public static string RemoveString(this string text, params string[] remove)
        {
            if (string.IsNullOrWhiteSpace(text) || remove.Length == 0) { return text; }

            remove.ToList().ForEach(x =>
            {
                text = text.Replace(x, "");
            });

            return text;
        }
    }
}
