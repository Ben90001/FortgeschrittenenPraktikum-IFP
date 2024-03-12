using NUnit.Framework;

public class TestTemplate
{
    public class AwakeMethodTests
    {
        [SetUp]
        public void BeforeEveryTest()
        {
            // Make basic setup here
            // Function gets called before every test
        }

        [Test]
        public void NameOfTest()
        {
            // Make Assertions here
        }
    }
}