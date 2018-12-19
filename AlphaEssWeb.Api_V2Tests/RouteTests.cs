//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq;
//using System.Web;
//using System.Web.Routing;
//using AlphaEssWeb.Api_V2.Config;
//using Xunit;
//using System.Web.Http;
//using System.Reflection;
//using System.Net.Http;

//namespace AlphaEssWeb.Api_V2Tests
//{
//	public class RouteTests
//	{
//		private HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
//		{
//			// create the mock request
//			Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
//			mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
//			mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);
//			// create the mock response
//			Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
//			mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
//			// create the mock context, using the request and response
//			Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
//			mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
//			mockContext.Setup(m => m.Response).Returns(mockResponse.Object);
//			//return the mocked context
//			//Mock<HttpRequestMessage> mockReM = new Mock<HttpRequestMessage>();

//			return mockContext.Object;
//		}

//		private void TestRouteMatch(string url, string controller, string action, object routeProperties = null, string httpMethod = "GET")
//		{
//			// Arrange
			
//			//RouteCollection routes = new RouteCollection();
//			RouteConfig.RegisterRoutes(GlobalConfiguration.Configuration);
//			// Act - process the route
//			RouteData result = GlobalConfiguration.Configuration.Routes.GetRouteData(CreateHttpContext(url, httpMethod));
//			// Assert
//			Assert.NotNull(result);
//			Assert.True(TestIncomingRouteResult(result, controller, action, routeProperties));
//		}

//		private bool TestIncomingRouteResult(RouteData routeResult, string controller, string action, object propertySet = null)
//		{
//			Func<object, object, bool> valCompare = (v1, v2) => {
//				return StringComparer.InvariantCultureIgnoreCase
//				.Compare(v1, v2) == 0;
//			};
//			bool result = valCompare(routeResult.Values["controller"], controller)
//			&& valCompare(routeResult.Values["action"], action);
//			if (propertySet != null)
//			{
//				PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
//				foreach (PropertyInfo pi in propInfo)
//				{
//					if (!(routeResult.Values.ContainsKey(pi.Name)
//					&& valCompare(routeResult.Values[pi.Name],
//					pi.GetValue(propertySet, null))))
//					{
//						result = false;
//						break;
//					}
//				}
//			}
//			return result;
//		}

//		private void TestRouteFail(string url)
//		{
//			// Arrange
//			RouteCollection routes = new RouteCollection();
//			GlobalConfiguration.Configuration.Routes
//			RouteConfig.RegisterRoutes(GlobalConfiguration.Configuration);
//			// Act - process the route
//			RouteData result = routes.GetRouteData(CreateHttpContext(url));
//			// Assert
//			Assert.True(result == null || result.Route == null);
//		}
//	}
//}
