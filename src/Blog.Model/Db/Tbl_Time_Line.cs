using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model.Db
{
    public class Tbl_Time_Line : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int Status { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime End_Time { get; set; }
    }
}
