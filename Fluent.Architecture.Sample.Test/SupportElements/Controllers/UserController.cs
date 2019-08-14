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
using Fluent.Architecture.Specifications;
using System.Collections.Generic;

namespace Fluent.Architecture.Sample.Test.SupportElements.Controllers
{
    public class UserController : FluentController<User>
    {
        public new UserService Service => base.Service as UserService;

        public User Add(User entity)
        {
            return Service.Add(entity);
        }

        public User[] AddRange(User[] entities)
        {
            Service.AddRange(entities);
            return entities;
        }

        public User Remove(User entity)
        {
            return Service.Remove(entity);
        }

        public User[] RemoveRange(User[] entities)
        {
            Service.RemoveRange(entities);
            return entities;
        }

        public void RemoveRange(string password)
        {
            var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
            Service.RemoveRange(spec);
        }

        public User Update(User entity)
        {
            return Service.Update(entity);
        }

        public User Find(User entity)
        {
            return Service.Find(entity);
        }

        public User UserByEmail(string email)
        {
            return Service.FirstOrDefault(CreateSpec<UserByEmailSpec>().DefineParams(email));
        }

        public User FindById(int id)
        {
            return Service.FirstOrDefault(CreateSpec<SpecFindById<User>>().DefineParameters(id));
        }

        public User SpecOne(string email)
        {
            var spec = CreateSpec<UserByEmailSpec>().DefineParams(email);
            return Service.FirstOrDefault(spec);
        }

        public int SpecOneInt(string email)
        {
            var spec = CreateSpec<UserIdByEmailSpec>().DefineParams(email);
            return Service.FirstOrDefaultSelect(spec);
        }

        public UserStudent SpecOneUserAndStudent(string email)
        {
            var spec = CreateSpec<UserAndStudentByEmailSpec>().DefineParams(email);
            return Service.FirstOrDefaultSelect(spec);
        }

        public User FirstOrDefault()
        {
            return Service.FirstOrDefault();
        }

        public int Count()
        {
            var spec = CreateSpec<AllSpec<User>>();
            return Service.Count(spec);
        }

        public int CountIdByPassword(string password)
        {
            var spec = CreateSpec<UserIdByPasswordSpec>().DefineParams(password);
            return Service.CountSelect(spec);
        }

        public int CountByPassword(string password)
        {
            var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
            return Service.Count(spec);
        }

        public bool ExistsByPassword(string password)
        {
            var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
            return Service.Exists(spec);
        }

        public bool ExistsIdByPassword(string password)
        {
            var spec = CreateSpec<UserIdByPasswordSpec>().DefineParams(password);
            return Service.Exists(spec);
        }

        public int CountByTelNumber(string number)
        {
            var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
            return Service.Count(spec);
        }

        public List<User> ListByTelNumber(string number)
        {
            var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
           return Service.List(spec);
        }

        public List<User> ListByTelNumber(string number, FluentPagination pagination)
        {
            var spec = CreateSpec<UserTelContainsNumberSpec>().DefineParams(number);
            return Service.List(spec, pagination);
        }

        public List<User> ListByPassword(string password)
        {
            var spec = CreateSpec<UserByPasswordSpec>().DefineParams(password);
            return Service.List(spec);
        }

        public List<int> ListByIdPassword(string password)
        {
            var spec = CreateSpec<UserIdByPasswordSpec>().DefineParams(password);
            return Service.ListSelect(spec);
        }

        public UserStudent GetUserByEmail(string email)
        {
            return Service.GetUserByEmail(email);
        }
    }
}


