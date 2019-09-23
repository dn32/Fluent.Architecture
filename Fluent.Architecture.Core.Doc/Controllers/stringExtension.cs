namespace Fluent.Architecture.Core.Doc.Controllers
{
    public static class stringExtension
    {
        public static string Remove(this string initialText, string removeText)
        {
            return initialText.Replace(removeText, "");
        }
    }
}
