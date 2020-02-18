using dn32.infra.Controllers;
using dn32.infra.Test.Mock;
using dn32.infra.Test.Mock.ControllerMock;
using Newtonsoft.Json;
using System;
using dn32.infra.dados;

namespace dn32.infra.Test
{
    public class DnTest<TModel> where TModel : DnEntidade, new()
    {
        public virtual DnAPIController<TModel> GetNewController()
        {
            return MockUtil.GetMockController<DnAPIController<TModel>>();
        }

        public virtual TController GetNewController<TController>() where TController : DnAPIController<TModel>
        {
            return MockUtil.GetMockController<TController>();
        }

        public virtual bool Remove(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, bool>(newController, (DnAPIController<TModel> controller) => controller.Remove(model).Result);
        }

        public virtual TModel Add(TModel model)
        {
            return Execute((DnAPIController<TModel> controller) => controller.Add(model).Result);
        }

        //public virtual string Schema()
        //{
        //    var newController = GetNewController();
        //    return TestUtil.Execute(newController, (DnAPIController<TModel> controller) => controller.Schema()) as string;
        //}

        public virtual bool Update(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, bool>(newController, (DnAPIController<TModel> controller) => controller.Update(model).Result);
        }

        public virtual TModel Add()
        {
            return Execute((DnAPIController<TModel> controller) => controller.Add(GetNew()).Result);
        }

        public virtual bool Exists(TModel model)
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, bool>(newController, (DnAPIController<TModel> controller) => controller.ExistsByEntityGet(model).Result);
        }

        public virtual TModel[] AddRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, TModel[]>(newController, (DnAPIController<TModel> controller) => controller.AddRange(models).Result);
        }

        public virtual TModel[] List()
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, TModel[]>(newController, (DnAPIController<TModel> controller) => controller.List().Result);
        }

        public virtual TModel[] List(Filtro[] filters)
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, TModel[]>(newController, (DnAPIController<TModel> controller) => controller.ListByFilterGet(filters).Result);
        }
        public virtual int Count()
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, int>(newController, (DnAPIController<TModel> controller) => controller.Count().Result);
        }

        public virtual int Count(Filtro[] filters)
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, int>(newController, (DnAPIController<TModel> controller) => controller.CountByFilter(filters).Result);
        }

        public virtual bool UpdateRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, bool>(newController, (DnAPIController<TModel> controller) => controller.UpdateRange(models).Result);
        }

        public virtual TModel Find(TModel model)
        {
            return Execute((DnAPIController<TModel> controller) => controller.FindByEntityPost(model).Result);
        }

        public virtual bool RemoveRange(TModel[] models)
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, bool>(newController, (DnAPIController<TModel> controller) => controller.RemoveRange(models).Result);
        }

        public virtual bool Truncate(string ERASE_ALL_DATA = "no")
        {
            var newController = GetNewController();
            return TestUtil.Execute<DnAPIController<TModel>, bool>(newController, (DnAPIController<TModel> controller) => controller.Truncate(ERASE_ALL_DATA).Result);
        }

        public virtual TModel GetNew()
        {
            throw new NotImplementedException();
            //   return new DnAPIController<TModel>().ExampleData();
        }

        public virtual TModel Execute(Func<DnAPIController<TModel>, ResultadoPadrao<TModel>> actionMethod)
        {
            var newController = GetNewController();
            newController.OnActionExecuting(MockActionExecutingContextFactory.Create(newController));
            var ret = actionMethod(newController);
            newController.OnActionExecuted(MockActionExecutedContextFactory.Create(newController));
            return JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(ret.Dados));
        }
    }
}
