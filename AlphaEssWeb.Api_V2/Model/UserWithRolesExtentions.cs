using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Model
{
	public static class UserWithRolesExtentions
	{
		public static SysUserDto ToSysUserDto(this UserWithRoles ur)
		{
			return new SysUserDto
			{
				UserName = ur.User.USERNAME,
				//Sn = ur.User.VT_SYSTEM.FirstOrDefault().SYS_SN,
				Address = ur.User.ADDRESS,
				CellPhone = ur.User.CELLPHONE,
				CityCode = ur.User.CITYCODE,
				ContactUser = ur.User.LINKMAN,
				CountryCode = ur.User.COUNTRYCODE,
				Email = ur.User.EMAIL,
				Id = ur.User.Key,
				LanguageCode = ur.User.LANGUAGECODE,
				PostCode = ur.User.POSTCODE,
        UserType = ur.User.USERTYPE,
				LicenseNo = ur.User.LICNO,
				StateCode = ur.User.STATECODE
			};
		}

		public static SysUserDto ToSysUserDto(this UserWithRolesAndSystems urs)
		{
			return new SysUserDto
			{
				UserName = urs.User.USERNAME,
				Address = urs.User.ADDRESS,
				CellPhone = urs.User.CELLPHONE,
				CityCode = urs.User.CITYCODE,
				ContactUser = urs.User.LINKMAN,
				CountryCode = urs.User.COUNTRYCODE,
				Email = urs.User.EMAIL,
				Id = urs.User.Key,
				LanguageCode = urs.User.LANGUAGECODE,
				PostCode = urs.User.POSTCODE,
				AllowAutoUpdate = (urs.Systems != null && urs.Systems.Any() && urs.Systems.First().AllowAutoUpdate.HasValue ? urs.Systems.First().AllowAutoUpdate.Value.ToString() : "1"),
				UserType = urs.User.USERTYPE,
				LicenseNo = urs.User.LICNO,
				StateCode = urs.User.STATECODE
			};
		}
	}
}
