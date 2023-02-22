using Xunit;

namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            Math math = new Math();
            int a = 5, b = 6;
            int expected = 11;

            //Act
            int actual = math.Add(a, b);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}