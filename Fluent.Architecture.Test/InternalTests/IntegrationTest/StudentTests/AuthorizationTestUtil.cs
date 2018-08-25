namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.StudentTests
{
    public static class StudentTestUtil
    {
        public static Student GetNew()
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
