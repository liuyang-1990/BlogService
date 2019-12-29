namespace Blog.Model.Response
{
    public class ResultModel<T>
    {
        public T ResultInfo { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }
}