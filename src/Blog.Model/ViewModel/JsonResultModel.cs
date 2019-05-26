using System.Collections.Generic;

namespace Blog.Model.ViewModel
{
    public class JsonResultModel<T>
    {

        public IEnumerable<T> Rows { get; set; }

        public int TotalRows { get; set; }

    }
}