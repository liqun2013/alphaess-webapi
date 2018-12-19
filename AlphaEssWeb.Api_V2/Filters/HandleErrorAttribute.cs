using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace AlphaEssWeb.Api_V2.Filters
{
	public class HandleErrorAttribute: ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			actionExecutedContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
		}
	}
}
