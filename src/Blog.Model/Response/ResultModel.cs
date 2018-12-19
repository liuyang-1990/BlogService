namespace Blog.Model.Response
{
    public class ResultModel<T> : BaseResponse
    {
        public T ResultInfo { get; set; }
    }
}