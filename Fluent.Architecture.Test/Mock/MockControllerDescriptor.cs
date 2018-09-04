
using System;
using System.Web.Mvc;

namespace Fluent.Architecture.Test.Mock
{
    public class MockControllerDescriptor : ControllerDescriptor
    {
        public MockControllerDescriptor(Type controllerType)
        {
            this.ControllerType = controllerType;
        }

        public override ActionDescriptor FindAction(ControllerContext controllerContext, string actionName)
        {
            throw new NotImplementedException();
        }

        public override ActionDescriptor[] GetCanonicalActions()
        {
            throw new NotImplementedException();
        }

        public override Type ControllerType { get; }
    }
}

