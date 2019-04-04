

using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Sample.Test.SupportElements.Services
{
    public class FluentGlobalizationService : GlobalizationService
    {
        public override string GetResource(string key, string defaultMessage, params string[] parameters)
        {
            //if(key == "ThePropertyMustHaveAValueForThisOperation")
            //{
            //    return string.Format("Informe o {0}", parameters);
            //}

            return defaultMessage.Replace("_", " ").TitleCase();
        }
    }
}
