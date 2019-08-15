// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Controllers;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Services;
using Fluent.Architecture.Sample.Test.SupportElements.Specifications;
using System.Collections.Generic;

namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
{
    public class UserController : FluentAPIController<User>
    {
        public new UserService Service => base.Service as UserService;

        //public DefaultResult ListIdByZipCode(long zipCode)
        //{
        //    var spec = CreateSpec<UserIdByZipCodeSpec>().DefineParams(zipCode);
        //    return Result(Service.ListSelect(spec));
        //}

        //public User UserByEmail(string email)
        //{
        //    return Service.FirstOrDefault(CreateSpec<UserByEmailSpec>().DefineParams(email));
        //}
      
        public DefaultResult SpecOneUserAndStudent(string email)
        {
            var spec = CreateSpec<UserAndStudentByEmailSpec>().DefineParams(email);
            return Result(Service.FirstOrDefaultSelect(spec));
        }

        //public int CountByPassword(string password)
        //{
        //    var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
        //    return Service.Count(spec);
        //}

        //public bool ExistsByPassword(string password)
        //{
        //    var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
        //    return Service.Exists(spec);
        //}

        //public bool ExistsIdByPassword(string password)
        //{
        //    var spec = CreateSpec<UserIdByZipCodeSpec>().DefineParams(password);
        //    return Service.Exists(spec);
        //}

        //public int CountByTelNumber(string number)
        //{
        //    var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
        //    return Service.Count(spec);
        //}

        //public List<User> ListByTelNumber(string number)
        //{
        //    var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
        //    return Service.List(spec);
        //}

        //public List<User> ListByTelNumber(string number, FluentPagination pagination)
        //{
        //    var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
        //    return Service.List(spec, pagination);
        //}

        //public List<User> ListByPassword(string password)
        //{
        //    var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
        //    return Service.List(spec);
        //}

        //public List<int> ListByIdPassword(string password)
        //{
        //    var spec = CreateSpec<UserIdByZipCodeSpec>().DefineParams(password);
        //    return Service.ListSelect(spec);
        //}

        //public DefaultResult GetUserByEmail(string email)
        //{
        //    return Result(Service.GetUserByEmail(email));
        //}
    }
}


