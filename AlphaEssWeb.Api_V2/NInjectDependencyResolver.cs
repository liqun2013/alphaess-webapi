using Ninject;
using System.Web.Http.Dependencies;

namespace AlphaEssWeb.Api_V2
{
	public class NInjectDependencyResolver : NInjectDependencyScope, IDependencyResolver
	{
		private IKernel _kernel;
		public NInjectDependencyResolver(IKernel kernel) : base(kernel)
		{
			_kernel = kernel;
		}
		public IDependencyScope BeginScope()
		{
			return new NInjectDependencyScope(_kernel.BeginBlock());
		}
	}
}
