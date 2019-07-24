using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NLog;
using NLog.LayoutRenderers;

namespace Blog.Api.Log
{
    /// <summary>
    /// https://github.com/NLog/NLog/wiki/How-to-write-a-custom-layout-renderer
    /// </summary>
    [LayoutRenderer("BlogStandard")]
    public class BlogLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {

            builder.AppendLine("<Log>");
            builder.AppendLine($"<timestamp>{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}</timestamp>");
            builder.AppendLine($"<message>{this.BuildMsg(logEvent)}</message>");
            builder.AppendLine($"<category>{logEvent.LoggerName}</category>");
            builder.AppendLine($"<priority>0</priority>");

            logEvent.Properties.TryGetValue("EventId_Id", out var eventId);
            builder.AppendLine($"<eventId>{eventId}</eventId>");
            builder.AppendLine($"<serverity>{this.BuildLogLevel(logEvent.Level.ToString())}</serverity>");
            builder.AppendLine($"<machine>{Environment.MachineName}</machine>");
            builder.AppendLine($"<applicationDomain>{AppDomain.CurrentDomain.FriendlyName}</applicationDomain>");

            var process = Process.GetCurrentProcess();
            builder.AppendLine($"<processId>{process.Id.ToString()}</processId>");
            builder.AppendLine($"<processName>{process.ProcessName}</processName>");
            builder.AppendLine($"<threadName>{Thread.CurrentThread.Name}</threadName>");
            builder.AppendLine("</Log>");
            builder.AppendLine("<!------------------------------------------------------------------------------------>");
            builder.AppendLine("");
        }

        /// <summary>
        /// Build log level
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        private string BuildLogLevel(string logLevel)
        {
            switch (logLevel)
            {
                case "Trace":
                case "Debug":
                    return "Verbose";
                case "Info":
                    return "Information";
                case "Warn":
                    return "Warning";
                case "Error":
                    return "Error";
                case "Fatal":
                    return "Critical";
                default:
                    return "Information";
            }
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <returns>The message.</returns>
        /// <param name="logEvent">Log event.</param>
        private string BuildMsg(LogEventInfo logEvent)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Message:{logEvent.FormattedMessage}");
            builder.AppendLine($"Exception:{logEvent.Exception?.ToString()}");

            return builder.ToString();
        }
    }
}
