using Blog.Infrastructure.Implement;
using Xunit;

namespace Blog.Test.Infrastructure
{
    public class BlogUrlHelperUnitTest
    {
        [Theory]
        [InlineData("", "", null)]
        [InlineData("a=3&b=4", "b", "4")]
        [InlineData("a=3&b=4", "c", null)]
        public void GetQueryString_test(string url, string para, string result)
        {
            var helper = new BlogUrlHelper();
            var actual = helper.GetQueryString(url, para);
            Assert.Equal(result, actual);
        }
    }
}
