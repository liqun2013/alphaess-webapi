using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class SysMenuDto: IDto<Guid>
	{
		public Guid Id { get; set; }
		public string MenuName { get; set; }
		public string MenuName_en { get; set; }
		public string MenuName_de { get; set; }
		public string MenuUrl { get; set; }
		public Guid? ParentId { get; set; }
		public int? OrderIndex { get; set; }
		public string CreateAccount { get; set; }
		public DateTime? CreateDatetime { get; set; }
		public string UpdateAccount { get; set; }
		public DateTime? UpdateDatetime { get; set; }
		/// <summary>
		/// 对应Attr1字段
		/// </summary>
		public string MenuOrTab { get; set; }
		/// <summary>
		/// 法语名
		/// 对应Attr2字段
		/// </summary>
		public string MenuName_fa { get; set; }
		/// <summary>
		/// 日语名
		/// 对应Attr3字段
		/// </summary>
		public string MenuName_ja { get; set; }
		public IEnumerable<SysMenuDto> SubMenus { get; set; }
		public IEnumerable<SysRoleDto> Roles { get; set; }
	}
}
