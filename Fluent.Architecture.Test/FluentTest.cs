using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Test.Mock.ControllerMock;
using Newtonsoft.Json;
using System;

namespace Fluent.Architecture.Test
{
    public class FluentTest<TModel> where TModel : FluentEntity, new()
    {  
        protected virtual FluentAPIController<TModel> GetNewController()
        {
            return MockUtil.GetMockController<FluentAPIController<TModel>>();
        }

        protected virtual TController GetNewController<TController>() where TController : FluentAPIController<TModel>
        {
            return MockUtil.GetMockController<TController>();
        }

        protected virtual bool Remove(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.Remove(model));
        }

        protected virtual TModel Add(TModel model)
        {
            return Execute((FluentAPIController<TModel> controller) => controller.Add(model));
        }

        protected virtual bool Update(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.Update(model));
        }

        protected virtual TModel Add()
        {
            return Execute((FluentAPIController<TModel> controller) => controller.Add(GetNew()));
        }

        protected virtual bool Exists(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.Exists(model));
        }

        protected virtual TModel[] AddRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.AddRange(models));
        }

        protected virtual TModel[] List()
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.List());
        }

        protected virtual TModel[] List(Filter[] filters)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.List(filters));
        }
        protected virtual int Count()
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, int>(newController, (FluentAPIController<TModel> controller) => controller.Count());
        }

        protected virtual int Count(Filter[] filters)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, int>(newController, (FluentAPIController<TModel> controller) => controller.Count(filters));
        }

        protected virtual bool UpdateRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.UpdateRange(models));
        }

        protected virtual TModel Find(TModel model)
        {
            return Execute((FluentAPIController<TModel> controller) => controller.Find(model));
        }

        protected virtual bool RemoveRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.RemoveRange(models));
        }

        protected virtual TModel GetNew()
        {
            return new FluentAPIController<TModel>().ExampleData();
        }

        protected virtual TModel Execute(Func<FluentAPIController<TModel>, DefaultResult> actionMethod)
        {
            var newController = GetNewController();
            newController.OnActionExecuting(MockActionExecutingContextFactory.Create(newController));
            var ret = actionMethod(newController);
            newController.OnActionExecuted(MockActionExecutedContextFactory.Create(newController));
            return JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(ret.Data));
        }
    }
}
