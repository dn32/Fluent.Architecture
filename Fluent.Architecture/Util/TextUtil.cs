using System.Text;

namespace dn32.infra.Util
{
    public static class TextUtil
    {
        public static string ClearText(this string str, string separator = "")
        {
            if (string.IsNullOrWhiteSpace(str)) { return str; }
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append(separator);
                }
            }
            return sb.ToString();
        }
    }
}
