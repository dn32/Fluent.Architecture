using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentRandomKeyValueOnAddAttribute : Attribute
    {
        public int Max { get; set; }

        public FluentRandomKeyValueOnAddAttribute(int max = 0)
        {
            Max = max;
        }
    }
}
