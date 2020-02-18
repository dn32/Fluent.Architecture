using System;

namespace dn32.infra.Nucleo.Doc.Controllers
{
    public class DocParameter
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Example { get; set; }

        public EnumParameterSouce Source { get; set; }

        public string Link { get; internal set; }

        public DocParameter()
        {
            Source = EnumParameterSouce.Body;
        }

        public DocParameter(string name, Type type, EnumParameterSouce source, string description, string example)
        {
            Name = name;
            Type = type;
            Link = DnDocController.GetModelLink(Type);
            Source = source;
            Description = description;
            Example = example;
        }
    }
}