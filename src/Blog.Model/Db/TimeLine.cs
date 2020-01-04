using System;
using SqlSugar;

namespace Blog.Model.Db
{
    [SugarTable("tbl_time_line")]
    public class TimeLine : BaseEntity
    {
        [SugarColumn(Length = 50, IsNullable = false)]
        public string Title { get; set; }

        [SugarColumn(Length = 500, IsNullable = false)]
        public string Content { get; set; }

        public int Status { get; set; }

        [SugarColumn(ColumnName = "start_time")]
        public DateTime StartTime { get; set; }
        [SugarColumn(ColumnName = "end_time")]
        public DateTime EndTime { get; set; }
    }
}
