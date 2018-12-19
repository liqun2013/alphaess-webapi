using System.Collections.Generic;

namespace AlphaEss.Api_V2.Infrastructure
{
	public enum LanguageNames
	{
		English, ChineseS, French, German, Japanese, Spanish, Korean
	}

	public class LanguageCodeConstants
	{
		private Dictionary<string, string> dicLanguageCodes;

		private LanguageCodeConstants()
		{
			dicLanguageCodes = new Dictionary<string, string>();
			dicLanguageCodes.Add(LanguageNames.English.ToString(), "en");
			dicLanguageCodes.Add(LanguageNames.ChineseS.ToString(), "zh-CN");
			dicLanguageCodes.Add(LanguageNames.French.ToString(), "fr");
			dicLanguageCodes.Add(LanguageNames.German.ToString(), "de-DE");
			dicLanguageCodes.Add(LanguageNames.Japanese.ToString(), "ja");
			dicLanguageCodes.Add(LanguageNames.Spanish.ToString(), "es");
			dicLanguageCodes.Add(LanguageNames.Korean.ToString(), "ko");
		}

		private static LanguageCodeConstants Create()
		{
			return new LanguageCodeConstants();
		}

		public bool CheckLanguageCodeExist(string lanCode)
		{
			if (string.IsNullOrWhiteSpace(lanCode))
				return false;

			return dicLanguageCodes.ContainsValue(lanCode);
		}

		public static bool CheckLanguageCodeIsExist(string lanCode)
		{
			Singleton<LanguageCodeConstants> s = new Singleton<LanguageCodeConstants>(Create);
			return s.Value.CheckLanguageCodeExist(lanCode);
		}

		public string GetLanguageCode(LanguageNames lanName)
		{
			return GetLanguageCode(lanName.ToString());
		}

		public static string GetLanguageCodeByName(LanguageNames lanName)
		{
			Singleton<LanguageCodeConstants> s = new Singleton<LanguageCodeConstants>(Create);
			return s.Value.GetLanguageCode(lanName);
		}
		public string GetLanguageCode(string lanName)
		{
			var result = dicLanguageCodes[LanguageNames.English.ToString()];

			if (!string.IsNullOrWhiteSpace(lanName) && dicLanguageCodes.ContainsKey(lanName))
			{
				result = dicLanguageCodes[lanName];
			}

			return result;
		}

		public static string GetLanguageCodeByName(string lanName)
		{
			Singleton<LanguageCodeConstants> s = new Singleton<LanguageCodeConstants>(Create);
			return s.Value.GetLanguageCode(lanName);
		}
	}
}
