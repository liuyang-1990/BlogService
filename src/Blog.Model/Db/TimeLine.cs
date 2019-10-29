using System;
using SqlSugar;

namespace Blog.Model.Db
{
    [SugarTable("tbl_time_line")]
    public class TimeLine : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int Status { get; set; }

        [SugarColumn(ColumnName = "start_time")]
        public DateTime StartTime { get; set; }
        [SugarColumn(ColumnName = "end_time")]
        public DateTime EndTime { get; set; }
    }
}
