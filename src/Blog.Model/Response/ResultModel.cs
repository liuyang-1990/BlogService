namespace Blog.Model.Response
{
    public class ResultModel<T>
    {
        public bool IsSuccess { get; set; }

        public int Code { get; set; }

        public string Msg { get; set; }

        public T ResultInfo { get; set; }

    }
}