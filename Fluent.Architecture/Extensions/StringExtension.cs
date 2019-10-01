using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Fluent.Architecture.Extensions
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // if the next character is a space, which is not considered uppercase 
                    // (otherwise we wouldn't be here...)
                    // we want to ensure that the following:
                    // 'FOO bar' is rewritten as 'foo bar', and not as 'foO bar'
                    // The code was written in such a way that the first word in uppercase
                    // ends when if finds an uppercase letter followed by a lowercase letter.
                    // now a ' ' (space, (char)32) is considered not upper
                    // but in that case we still want our current character to become lowercase
                    if (char.IsSeparator(chars[i + 1]))
                    {
                        chars[i] = ToLower(chars[i]);
                    }

                    break;
                }

                chars[i] = ToLower(chars[i]);
            }

            return new string(chars);
        }

        private static char ToLower(char c)
        {
#if HAVE_CHAR_TO_STRING_WITH_CULTURE
            c = char.ToLower(c, CultureInfo.InvariantCulture);
#else
            c = char.ToLowerInvariant(c);
#endif
            return c;
        }

        //public static string TitleCase(this string text)
        //{
        //    return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLowerInvariant());
        //}

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

        //public static string RemoveString(this string text, params string[] remove)
        //{
        //    if (string.IsNullOrWhiteSpace(text) || remove.Length == 0) { return text; }

        //    remove.ToList().ForEach(x =>
        //    {
        //        text = text.Replace(x, "");
        //    });

        //    return text;
        //}
    }
}
