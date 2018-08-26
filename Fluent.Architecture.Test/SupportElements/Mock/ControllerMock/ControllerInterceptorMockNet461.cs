//#if NET461
//using System;
//using System.Collections.Concurrent;
//using System.Web.Mvc;
//using System.Web.Routing;
//using Fluent.Architecture.interceptors;

//namespace Fluent.Architecture.Test.Mock.ControllerMock
//{
//    internal class ControllerInterceptorMockNet461 : interceptors.FluentInterceptor
//    {
//        private BaseController Controller { get; set; }

//        public void SetMethods(BaseController controller)
//        {
//            Controller = controller;
//        }

//        public ControllerInterceptorMockNet461(Guid sessionId) //: base(sessionId)
//        {

//        }

//        public override void Intercept(FluentInvocation invocation)
//        {
//            //if (invocation.Method.Name == "OnActionExecuting")
//            //{
//            //    invocation.Proceed();
//            //    return;
//            //}

//            //MockInterceptedController(invocation);

//            //invocation.Proceed();
//        }

//        private void MockInterceptedController(FluentInvocation invocation)
//        {
//            var requestContext = CreateRequestContext(invocation, out ActionExecutingContext contextExecuting);

//            //Controller.InitializeControllerForMock(requestContext);

//            //Controller.OnActionExecutingForTest(contextExecuting);
//        }


//        private RequestContext CreateRequestContext(FluentInvocation invocation, out ActionExecutingContext contextExecuting)
//        {
//            var actionName = invocation.Method.Name;
//            var controllerType = invocation.TargetType;
//            var actionDescriptor = new MockActionDescriptor(actionName, controllerType);
//            Controller.ControllerContext = new ActionExecutedContext { ActionDescriptor = actionDescriptor };
//            contextExecuting = new ActionExecutingContext(Controller.ControllerContext, actionDescriptor, new ConcurrentDictionary<string, object>());

//            var response = new HttpResponseBaseMock();
//            var request = new HttpRequestBaseMock();
//            var contextExecutingMock = new HttpContextBaseMock(request, response);
//            var routeData = new RouteData();

//            contextExecuting.RequestContext = new RequestContext(contextExecutingMock, routeData);
//            var requestContext = new RequestContext { HttpContext = contextExecutingMock };

//            return requestContext;
//        }
//    }
//}
//#endif