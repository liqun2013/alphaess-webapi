using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaEss.Api_V2.Infrastructure.Tests
{
	[TestClass()]
	public class CryptoServiceTests
	{
		[TestMethod()]
		public void GenerateMD5HashTest()
		{
			var _cryptoService = new Mock<CryptoService>();
			var hash =_cryptoService.Object.GenerateMD5Hash("api_account =testemail=walker.ling@alpha-ess.comsecretkey=ALPHAESSWEBAPI201510timestamp=1469093194username=alphaess");

			Assert.IsNotNull(hash);
		}
	}
}