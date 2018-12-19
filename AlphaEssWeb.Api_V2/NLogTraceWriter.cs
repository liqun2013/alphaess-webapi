using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;
using AlphaEssWeb.Api_V2.Model;

namespace AlphaEssWeb.Api_V2
{
	public class NLogTraceWriter : ITraceWriter
	{
		public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
		{
			if (level == TraceLevel.Off) return;
			var record = new TraceRecord(request, category, level);
			traceAction(record);
			logToNLog(record);
		}

		private void logToNLog(TraceRecord traceRecord)
		{
			var messageBuilder = new StringBuilder();

			// traceRecord.Category
			// System.Web.Http.Request
			// System.Web.Http.Controllers
			// System.Web.Http.Action
			// System.Web.Http.ModelBinding
			// System.Net.Http.Formatting
			// System.Web.Http.Filters

			if (traceRecord.Category.Equals("System.Web.Http.Request", StringComparison.OrdinalIgnoreCase))
			{
				if (traceRecord.Request != null)
				{
					if (traceRecord.Request.Method != null && traceRecord.Category.Equals("System.Web.Http.Request", StringComparison.OrdinalIgnoreCase))
					{
						messageBuilder.Append(traceRecord.Request.Method);
					}
					if (traceRecord.Request.RequestUri != null && traceRecord.Category.Equals("System.Web.Http.Request", StringComparison.OrdinalIgnoreCase))
					{
						messageBuilder.Append(" " + traceRecord.Request.GetClientIpAddress() + " " + traceRecord.Request.RequestUri);
					}
					if (traceRecord.Request.Headers != null && traceRecord.Category.Equals("System.Web.Http.Request", StringComparison.OrdinalIgnoreCase))
					{
						var h = traceRecord.Request.Headers;
						var headers = new StringBuilder();

						if (h.Accept != null && !string.IsNullOrWhiteSpace(h.Accept.ToString()))
							headers.AppendFormat("Accept: {0}{1}", h.Accept.ToString(), Environment.NewLine);
						if (h.AcceptCharset != null && !string.IsNullOrWhiteSpace(h.AcceptCharset.ToString()))
							headers.AppendFormat("AcceptCharset: {0}{1}", h.AcceptCharset.ToString(), Environment.NewLine);
						if (h.AcceptEncoding != null && !string.IsNullOrWhiteSpace(h.AcceptEncoding.ToString()))
							headers.AppendFormat("AcceptEncoding: {0}{1}", h.AcceptEncoding.ToString(), Environment.NewLine);
						if (h.AcceptLanguage != null && !string.IsNullOrWhiteSpace(h.AcceptLanguage.ToString()))
							headers.AppendFormat("AcceptLanguage: {0}{1}", h.AcceptLanguage.ToString(), Environment.NewLine);
						if (h.UserAgent != null && !string.IsNullOrWhiteSpace(h.UserAgent.ToString()))
							headers.AppendFormat("UserAgent: {0}{1}", h.UserAgent.ToString(), Environment.NewLine);

						if (headers.Length > 0)
							messageBuilder.AppendFormat("{0}{1}", Environment.NewLine, headers.ToString());
					}
					if (traceRecord.Request.Content != null && traceRecord.Category.Equals("System.Web.Http.Request", StringComparison.OrdinalIgnoreCase))
					{
						var content = traceRecord.Request.Content.ReadAsStringAsync().Result;
						if(content.Length > 3000)
							messageBuilder.Append(Environment.NewLine + content.Substring(0, 3000) + "......");
						else
							messageBuilder.Append(Environment.NewLine + content);
					}
					if (traceRecord.Category.Equals("System.Web.Http.Request", StringComparison.OrdinalIgnoreCase))
						messageBuilder.AppendLine();
				}
				if (!string.IsNullOrWhiteSpace(traceRecord.Category))
				{
					messageBuilder.Append(" " + traceRecord.Category);
				}
				if (!string.IsNullOrWhiteSpace(traceRecord.Operator))
				{
					messageBuilder.Append(" " + traceRecord.Operator + " " + traceRecord.Operation);
				}
				if (!string.IsNullOrWhiteSpace(traceRecord.Message))
				{
					messageBuilder.Append(" " + traceRecord.Message);
				}
			}
			if (traceRecord.Exception != null)
			{
				var be = traceRecord.Exception.GetBaseException();
				if (be != null)
				{
					messageBuilder.AppendLine();
					messageBuilder.Append(be.Message);
					messageBuilder.Append(be.StackTrace);
				}

				if (traceRecord.Exception.InnerException != null)
				{
					messageBuilder.AppendLine();
					messageBuilder.Append(traceRecord.Exception.InnerException.Message);
					messageBuilder.Append(traceRecord.Exception.InnerException.StackTrace);
				}
			}
			if (messageBuilder.Length > 0)
				currentLogger[traceRecord.Level](messageBuilder.ToString());
		}

		private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> Loggers = new Lazy<Dictionary<TraceLevel, Action<string>>>(() =>
			new Dictionary<TraceLevel, Action<string>> {
			{ TraceLevel.Debug, LogManager.GetCurrentClassLogger().Debug },
			{ TraceLevel.Error, LogManager.GetCurrentClassLogger().Error },
			{ TraceLevel.Fatal, LogManager.GetCurrentClassLogger().Fatal },
			{ TraceLevel.Info, LogManager.GetCurrentClassLogger().Info },
			{ TraceLevel.Warn, LogManager.GetCurrentClassLogger().Warn }
		});
		private Dictionary<TraceLevel, Action<string>> currentLogger
		{
			get { return Loggers.Value; }
		}
	}
}
