using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace AlphaEssWeb.Api_V2
{
	public class NInjectDependencyScope : IDependencyScope
	{
		private IResolutionRoot _resolutionRoot;
		public NInjectDependencyScope(IResolutionRoot resolutionRoot)
		{
			_resolutionRoot = resolutionRoot;
		}

		public void Dispose()
		{
			var disposable = _resolutionRoot as IDisposable;
			if (disposable != null)
				disposable.Dispose();
			_resolutionRoot = null;
		}
		public object GetService(Type serviceType)
		{
			return GetServices(serviceType).FirstOrDefault();
		}
		public IEnumerable<object> GetServices(Type serviceType)
		{
			var request = _resolutionRoot.CreateRequest(serviceType, null, new IParameter[0], true, true);
			return _resolutionRoot.Resolve(request);
		}
	}

	//// This class is the resolver, but it is also the global scope
	//// so we derive from NinjectScope.
	//public class NinjectDependencyResolver : NInjectDependencyScope, IDependencyResolver
	//{
	//	IKernel kernel;

	//	public NinjectDependencyResolver(IKernel kernel) : base(kernel)
	//	{
	//		this.kernel = kernel;
	//	}

	//	public IDependencyScope BeginScope()
	//	{
	//		return new NinjectDependencyScope(kernel.BeginBlock());
	//	}
	//}
}
