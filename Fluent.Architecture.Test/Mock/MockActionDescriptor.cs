#if NET461
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fluent.Architecture.Test.Mock
{
    public class MockActionDescriptor : ActionDescriptor
    {
        
        public MockActionDescriptor(string actionName, Type controllerType)
        {
            ActionName = actionName;
            ControllerDescriptor = new MockControllerDescriptor(controllerType);
        }

        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            return null;
        }

        public override ParameterDescriptor[] GetParameters()
        {
            return new List<ParameterDescriptor>().ToArray();
        }

        public override string ActionName { get; }

        public override ControllerDescriptor ControllerDescriptor { get; }
    }
}
#endif