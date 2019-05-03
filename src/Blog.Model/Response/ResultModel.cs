namespace Blog.Model.Response
{
    public class ResultModel<T>
    {
        public T ResultInfo { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }

    public class ResultModel<T, T1>
    {
        public T ResultInfo { get; set; }

        public T1 Message { get; set; }

        public bool IsSuccess { get; set; }

        public string Status { get; set; }
    }
}