namespace Fluent.Architecture.Core.Doc.Controllers
{
    //Todo - Move to core
    public static class StringExtension
    {
        public static string Remove(this string initialText, string removeText)
        {
            return initialText.Replace(removeText, "");
        }
    }
}
