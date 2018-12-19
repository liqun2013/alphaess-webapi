using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AlphaEssWeb.Api_V2.Config
{
	public class NinjectWebCommon
	{
		public static void Initialize(HttpConfiguration config)
		{
			Initialize(config, RegisterServices(new StandardKernel()));
		}

		public static void Initialize(HttpConfiguration config, IKernel kernel)
		{
			config.DependencyResolver = new NInjectDependencyResolver(kernel);
		}

		private static IKernel RegisterServices(IKernel kernel)
		{


			return kernel;
		}
	}
}
