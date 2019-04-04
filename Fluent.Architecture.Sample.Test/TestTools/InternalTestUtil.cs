// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System.Runtime.InteropServices;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;

namespace Fluent.Architecture.Sample.Test.TestTools
{

    [ComVisible(true)]
    public static class InternalTestUtil
    {
        public static Course GetNewCourse()
        {
            var rand = TestUtil.NextRandom();
            return new Course
            {
                Id = rand,
                Title = $"Title {rand}",
                Description = $"Description { rand }"
            };
        }

        public static User GetNewUser()
        {
            var rand = TestUtil.NextRandom();
            return new User
            {
                PersonType = ePersonType.User,
                Id = rand,
                UserName = $"maria {rand}",
                Name = $"maria {rand}",
                Email = $"test{rand}@mail.com",
                Password = $"test{rand}@mail.com",
                Tel = $"test{rand}@mail.com",
            };
        }

        public static Student GetNewStudent()
        {
            var rand = TestUtil.NextRandom();
            return new Student
            {
                Name = $"Name {rand}",
                Document = $"Document {rand}",
                Email = $"Email {rand}"
            };
        }
    }
}
