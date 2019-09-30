using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Controllers.ControllerModel;
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
        public virtual FluentAPIController<TModel> GetNewController()
        {
            return MockUtil.GetMockController<FluentAPIController<TModel>>();
        }

        public virtual TController GetNewController<TController>() where TController : FluentAPIController<TModel>
        {
            return MockUtil.GetMockController<TController>();
        }

        public virtual bool Remove(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.Remove(model).Result);
        }

        public virtual TModel Add(TModel model)
        {
            return Execute((FluentAPIController<TModel> controller) => controller.Add(model).Result);
        }

        //public virtual string Schema()
        //{
        //    var newController = GetNewController();
        //    return TestUtil.Execute(newController, (FluentAPIController<TModel> controller) => controller.Schema()) as string;
        //}

        public virtual bool Update(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.Update(model).Result);
        }

        public virtual bool UpdateAlter(UpdateAlter<TModel> model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.UpdateAlter(model).Result);
        }

        public virtual TModel Add()
        {
            return Execute((FluentAPIController<TModel> controller) => controller.Add(GetNew()).Result);
        }

        public virtual bool Exists(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.ExistsByEntityGet(model).Result);
        }

        public virtual TModel[] AddRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.AddRange(models).Result);
        }

        public virtual TModel[] List()
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.List().Result);
        }

        public virtual TModel[] List(Filter[] filters)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.ListByFilterGet(filters).Result);
        }
        public virtual int Count()
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, int>(newController, (FluentAPIController<TModel> controller) => controller.Count().Result);
        }

        public virtual int Count(Filter[] filters)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, int>(newController, (FluentAPIController<TModel> controller) => controller.CountByFilter(filters).Result);
        }

        public virtual bool UpdateRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.UpdateRange(models).Result);
        }

        public virtual TModel Find(TModel model)
        {
            return Execute((FluentAPIController<TModel> controller) => controller.FindByEntityPost(model).Result);
        }

        public virtual bool RemoveRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.RemoveRange(models).Result);
        }

        public virtual bool Truncate(string ERASE_ALL_DATA = "no")
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.Truncate(ERASE_ALL_DATA).Result);
        }

        public virtual TModel GetNew()
        {
            return new FluentAPIController<TModel>().ExampleData();
        }

        public virtual TModel Execute(Func<FluentAPIController<TModel>, DefaultResult> actionMethod)
        {
            var newController = GetNewController();
            newController.OnActionExecuting(MockActionExecutingContextFactory.Create(newController));
            var ret = actionMethod(newController);
            newController.OnActionExecuted(MockActionExecutedContextFactory.Create(newController));
            return JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(ret.Data));
        }
    }
}
