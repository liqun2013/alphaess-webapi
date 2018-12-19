using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AlphaEss.Api_V2.Infrastructure;

namespace AlphaEssWeb.Api_V2.Domain
{
	public class VtColdataRepository : EntityRepository<VT_COLDATA, Guid>, IVtColdataRepository
	{
		public VtColdataRepository() : base(new AlphaEssDbContext()) { }

	}
}
