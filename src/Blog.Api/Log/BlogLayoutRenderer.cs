using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace Blog.Api.Log
{
    /// <summary>
    /// https://github.com/NLog/NLog/wiki/How-to-write-a-custom-layout-renderer
    /// </summary>
    [LayoutRenderer("File")]
    public class BlogLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {

            builder.AppendLine("----------blog log----------");
            builder.Append("Level:" + logEvent.Level.ToString());
            builder.Append("time:" + logEvent.TimeStamp);
            builder.Append("Message:" + logEvent.Message);
            builder.Append("StackTrace:" + logEvent.StackTrace.ToString());
            builder.AppendLine("----------------------------");
        }
    }
}
