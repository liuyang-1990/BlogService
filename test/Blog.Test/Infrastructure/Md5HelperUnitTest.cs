using Blog.Infrastructure.Implement;
using Xunit;

namespace Blog.Test.Infrastructure
{
    public class Md5HelperUnitTest
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("123", "202cb962ac59075b964b07152d234b70")]
        public void Encrypt32_Test(string originStr, string result)
        {
            var helper = new Md5Helper();
            var actual = helper.Encrypt32(originStr);
            Assert.Equal(result, actual);
        }
    }
}