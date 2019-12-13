using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Models;
using System;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentCompositionAttribute : FluentReferenceAttribute
    {
        public FluentJsonSchema Form { get; set; }
        public EnumOnSaveReference OnSave { get; set; } = EnumOnSaveReference.ADD_AND_UPDATE;
    }
}
