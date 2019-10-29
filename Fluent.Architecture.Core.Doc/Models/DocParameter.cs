using System;

namespace Fluent.Architecture.Core.Doc.Controllers
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
            Source = source;
            Description = description;
            Example = example;
        }
    }
}