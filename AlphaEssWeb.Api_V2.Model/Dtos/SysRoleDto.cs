using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class SysRoleDto: IDto<Guid>
	{
		public Guid Id { get; set; }
		public string RoleName { get; set; }
		public string RoleDescription { get; set; }
		public string CreateAccount { get; set; }
		public DateTime? CreateDatetime { get; set; }
		public string UpdateAccount { get; set; }
		public DateTime? UpdateDatetime { get; set; }

		public IEnumerable<SysMenuDto> Menus { get; set; }
		public IEnumerable<SysUserDto> Users { get; set; }
	}
}
