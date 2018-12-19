using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaEssWeb.Api_V2
{
	public class AlphaEssMessageHandler : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// Only for POST request method 
			if (request.Method != HttpMethod.Post)
			{
				TaskCompletionSource<HttpResponseMessage> tcs = new TaskCompletionSource<HttpResponseMessage>();
				tcs.SetResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "The api supports only the Post method." });
				return tcs.Task;
			}

			// Check for POST request and empty body.
			var content = request.Content.ReadAsStringAsync().Result;
			if (0 == content.Length)
			{
				TaskCompletionSource<HttpResponseMessage> tcs = new TaskCompletionSource<HttpResponseMessage>();
				tcs.SetResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Empty body not allowed for POST." });
				return tcs.Task;
			}
			return base.SendAsync(request, cancellationToken);
		}
	}
}
