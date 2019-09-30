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
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.RemoveAsync(model));
        }

        public virtual TModel Add(TModel model)
        {
            return Execute((FluentAPIController<TModel> controller) => controller.AddAsync(model));
        }

        //public virtual string Schema()
        //{
        //    var newController = GetNewController();
        //    return TestUtil.Execute(newController, (FluentAPIController<TModel> controller) => controller.Schema()) as string;
        //}

        public virtual bool Update(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.UpdateAsync(model));
        }

        public virtual bool UpdateAlter(UpdateAlter<TModel> model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.UpdateAlterAsync(model));
        }

        public virtual TModel Add()
        {
            return Execute((FluentAPIController<TModel> controller) => controller.AddAsync(GetNew()));
        }

        public virtual bool Exists(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.ExistsByEntityGetAsync(model));
        }

        public virtual TModel[] AddRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.AddRangeAsync(models));
        }

        public virtual TModel[] List()
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.ListAsync());
        }

        public virtual TModel[] List(Filter[] filters)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, TModel[]>(newController, (FluentAPIController<TModel> controller) => controller.ListByFilterGetAsync(filters));
        }
        public virtual int Count()
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, int>(newController, (FluentAPIController<TModel> controller) => controller.CountAsync());
        }

        public virtual int Count(Filter[] filters)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, int>(newController, (FluentAPIController<TModel> controller) => controller.CountByFilterAsync(filters));
        }

        public virtual bool UpdateRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.UpdateRangeAsync(models));
        }

        public virtual TModel Find(TModel model)
        {
            return Execute((FluentAPIController<TModel> controller) => controller.FindByEntityPost(model).Result);
        }

        public virtual bool RemoveRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.RemoveRangeAsync(models));
        }

        public virtual bool Truncate(string ERASE_ALL_DATA = "no")
        {
            var newController = GetNewController();
            return TestUtil.Execute<FluentAPIController<TModel>, bool>(newController, (FluentAPIController<TModel> controller) => controller.TruncateAsync(ERASE_ALL_DATA));
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
            newController.OnActionExecutedAsync(MockActionExecutedContextFactory.Create(newController));
            return JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(ret.Data));
        }
    }
}
