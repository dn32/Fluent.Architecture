using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Filters;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.Architecture.Controller.Test.POScope
{
    internal class UserControllerTest : InternalUserTestBase
    {
        [Test]
        public void AddSucess()
        {
            //Preparation
            var user = GetNewUser();

            //Operation
            var result = Execute<User>((UserController controller) => controller.Add(user));
            user.Id = result.Id;

            //Tests
            Assert.IsNotNull(result);
            Assert.Greater(result.Id, 0);
            FluentAssert.Equal(user, result);

            //Clear
            RemoveUser(user);
        }

        [Test]
        public void FindByIdSucess()
        {
            //Preparation
            var user = AddNewUser();

            //Operation
            var result = Execute<User>((UserController controller) => controller.Find(new User { Id = user.Id }));

            //Tests
            FluentAssert.Equal(user, result);

            //Clear
            RemoveUser(user);
        }

        [Test]
        public void FindByFluentUniqueKeySucess()
        {
            //Preparation
            var user = AddNewUser();

            //Operation
            var result = Execute<User>((UserController controller) => controller.Find(new User { Email = user.Email }));

            //Tests
            FluentAssert.Equal(user, result);

            //Clear
            RemoveUser(user);
        }

        [Test]
        public void ListSucess()
        {
            //Preparation
            var currentController = GetNewController();
            var user1 = AddNewUser();
            var user2 = AddNewUser();
            var user3 = AddNewUser();

            //Operation
            var result = TestUtil.Execute(currentController, (UserController controller) => controller.List()) as DefaultPaginationResult;

            //Tests
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Pagination);

            var list = result.Data.JsonObjectToObject<List<User>>();
            Assert.IsNotNull(list);
            FluentAssert.Equal(user1, list.FirstOrDefault(x => x.Id == user1.Id));
            FluentAssert.Equal(user2, list.FirstOrDefault(x => x.Id == user2.Id));
            FluentAssert.Equal(user3, list.FirstOrDefault(x => x.Id == user3.Id));

            //Clear
            RemoveUser(user1);
            RemoveUser(user2);
            RemoveUser(user3);
        }

        [Test]
        public void ListDefaultPaginationSucess()
        {
            //Preparation
            var currentController = GetNewController();
            var category = TestUtil.NextRandom();
            List<User> users = new List<User>();
            for (int i = 0; i < 40; i++)
            {
                var user = GetNewUser();
                user.Category = category;
                AddUser(user);
                users.Add(user);
            }
            var filters = new Filter[]
             {
                    new Filter
                    {
                        FilterType = EnumFilterType.EQUAL,
                        PropertyName = nameof(User.Category),
                        Value = category.ToString()
                    }
             };

            //Operation
            var result = TestUtil.Execute(currentController, (UserController controller) => controller.List(filters)) as DefaultPaginationResult;

            //Tests
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Pagination);
            var list = result.Data.JsonObjectToObject<List<User>>();
            Assert.IsNotNull(list);
            Assert.AreEqual(20, list.Count);

            Assert.AreEqual(0, result.Pagination.CurrentPage);
            Assert.AreEqual(20, result.Pagination.ItemsPerPage);
            Assert.AreEqual(2, result.Pagination.NumberOfPages);
            Assert.AreEqual(0, result.Pagination.Skip);
            Assert.AreEqual(true, result.Pagination.StartAtZero);
            Assert.AreEqual(40, result.Pagination.TotalQuantityOfItems);

            //Clear
            foreach (var user in users)
            {
                RemoveUser(user);
            }
        }

        [Theory]
        [TestCase(0, 5, 2, 0, true, 5)]
        [TestCase(1, 5, 2, 0, false, 5)]
        [TestCase(2, 5, 2, 5, false, 5)]
        [TestCase(0, 9, 2, 0, true, 9)]
        [TestCase(1, 9, 2, 9, true, 1)]
        public void ListSetPaginationSucess(
            int currentPage,
            int itemsPerPage,
            int numberOfPages,
            int skip,
            bool startAtZero,
            int currentQuantityOfItems)
        {
            /* A categoria foi usada para não conflitar os testes simultâneos que são executados aqui*/

            //Preparation
            var category = TestUtil.NextRandom();
            var users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                var user = GetNewUser();
                user.Category = category;
                AddUser(user);
                users.Add(user);
            }

            var customController = MockUtil.GetMockController<UserController>(new HeaderDictionary
            {
                { "currentPage", currentPage.ToString() },
                { "itemsPerPage", itemsPerPage.ToString() },
                { "startAtZero", startAtZero.ToString() }
            });

            var filters = new Filter[]
            {
                new Filter
                {
                    FilterType = EnumFilterType.EQUAL,
                    PropertyName = nameof(User.Category),
                    Value = category.ToString()
                }
            };

            //Operation
            var result = TestUtil.Execute(customController, (UserController controller) => controller.List(filters)) as DefaultPaginationResult;

            //Tests
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Pagination);
            var list = result.Data.JsonObjectToObject<List<User>>();
            Assert.IsNotNull(list);
            Assert.AreEqual(currentQuantityOfItems, list.Count);

            Assert.AreEqual(currentPage, result.Pagination.CurrentPage);
            Assert.AreEqual(itemsPerPage, result.Pagination.ItemsPerPage);
            Assert.AreEqual(numberOfPages, result.Pagination.NumberOfPages);
            Assert.AreEqual(skip, result.Pagination.Skip);
            Assert.AreEqual(startAtZero, result.Pagination.StartAtZero);
            Assert.AreEqual(10, result.Pagination.TotalQuantityOfItems);

            //Clear
            foreach (var user in users)
            {
                RemoveUser(user);
            }
        }

        [Theory]
        [TestCase(EnumFilterType.EQUAL, nameof(User.ZipCode), "65000", false, 2)]
        [TestCase(EnumFilterType.EQUAL, nameof(User.PersonType), "ExternalUser", false, 1)]
        [TestCase(EnumFilterType.EQUAL, nameof(User.HasChildren), "true", false, 2)]
        [TestCase(EnumFilterType.EQUAL, nameof(User.HasChildren), "false", false, 1)]

        [TestCase(EnumFilterType.TRUE, nameof(User.HasChildren), "", false, 2)]
        [TestCase(EnumFilterType.FALSE, nameof(User.HasChildren), "", false, 1)]

        [TestCase(EnumFilterType.GREATER, nameof(User.DateOfBirth), "24/01/2000", false, 1)]
        [TestCase(EnumFilterType.GREATER, nameof(User.DateOfBirth), "24/01/1980", false, 3)]
        [TestCase(EnumFilterType.SMALLER, nameof(User.DateOfBirth), "24/01/2000", false, 2)]

        [TestCase(EnumFilterType.GREATER, nameof(User.Age), "14", false, 2)]
        [TestCase(EnumFilterType.GREATER, nameof(User.Age), "25", false, 1)]
        [TestCase(EnumFilterType.GREATER, nameof(User.Age), "14", true, 3)]
        [TestCase(EnumFilterType.SMALLER, nameof(User.Age), "32", true, 3)]

        [TestCase(EnumFilterType.CONTAINS, nameof(User.Name), "Smit", false, 1)]
        [TestCase(EnumFilterType.CONTAINS, nameof(User.Name), "xxxy", false, 0)]

        [TestCase(EnumFilterType.START_WITH, nameof(User.Name), "Mariah", false, 1)]
        [TestCase(EnumFilterType.START_WITH, nameof(User.Name), "xxxy", false, 0)]

        [TestCase(EnumFilterType.ENDS_WITH, nameof(User.Name), "Santos", false, 1)]
        [TestCase(EnumFilterType.ENDS_WITH, nameof(User.Name), "xxxy", false, 0)]

        [TestCase(EnumFilterType.NULL, nameof(User.Password), "", false, 1)]

        public void ListSetFilterSucess(
            EnumFilterType filterType,
            string propertyName,
            string value,
            bool including,
            int count
            )
        {
            //Preparation
            var currentController = GetNewController();
            var users = new List<User>();
            var category = TestUtil.NextRandom();

            {
                var user = GetNewUser();
                user.ZipCode = 65000;
                user.Name = "John Smith";
                user.Age = 32;
                user.PersonType = EnumPersonType.ExternalUser;
                user.HasChildren = true;
                user.Category = category;
                user.DateOfBirth = DateTime.UtcNow.Date.AddYears(-1 * user.Age);
                user.Password = null;
                users.Add(user);
                AddUser(user);
            }
            {
                var user = GetNewUser();
                user.ZipCode = 65000;
                user.Name = "Mariah Theofly";
                user.Age = 25;
                user.PersonType = EnumPersonType.User;
                user.HasChildren = true;
                user.Category = category;
                user.DateOfBirth = DateTime.UtcNow.Date.AddYears(-1 * user.Age);
                users.Add(user);
                AddUser(user);
            }

            {
                var user = GetNewUser();
                user.ZipCode = 65001;
                user.Name = "Padro Santos";
                user.Age = 14;
                user.PersonType = EnumPersonType.User;
                user.HasChildren = false;
                user.Category = category;
                user.DateOfBirth = DateTime.UtcNow.Date.AddYears(-1 * user.Age);
                users.Add(user);
                AddUser(user);
            }

            var filters = new Filter[]
            {
                new Filter
                {
                    FilterType = filterType,
                    PropertyName = propertyName,
                    Value = value,
                    Including = including,
                    JunctionType = EnumJunctionType.AND
                },
                new Filter
                {
                    FilterType = EnumFilterType.EQUAL,
                    PropertyName = nameof(User.Category),
                    Value = category.ToString()
                }
            };

            //Operation
            {
                var result = TestUtil.Execute(currentController, (UserController controller) => controller.List(filters)) as DefaultPaginationResult;

                //Tests
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);
                Assert.IsNotNull(result.Pagination);
                var list = result.Data.JsonObjectToObject<List<User>>();
                Assert.IsNotNull(list);
                Assert.AreEqual(count, result.Pagination.TotalQuantityOfItems);
            }

            //Operation Reverse
            {
                filters[0].IsReverse = true;
                var result = TestUtil.Execute(currentController, (UserController controller) => controller.List(filters)) as DefaultPaginationResult;

                //Tests Reverse
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);
                Assert.IsNotNull(result.Pagination);
                var list = result.Data.JsonObjectToObject<List<User>>();
                Assert.IsNotNull(list);
                Assert.AreEqual(count, 3 - result.Pagination.TotalQuantityOfItems);
            }

            //Clear
            foreach (var user in users)
            {
                RemoveUser(user);
            }
        }

        [Test]
        public void Test()
        {

        }
    }
}
