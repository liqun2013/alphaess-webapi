using AlphaEssWeb.Api_V2.Model;
using System;
using System.Web.Helpers;

namespace AlphaEssWeb.Api_V2.Domain
{
	public static class TokenHelper
	{
		public static int ExpirationTime
		{
			get
			{
				var t = 0;
				if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ExpirationTime"], out t))
					return t;
				else
					return 90;
			}
		}

		public static void ClearToken(Guid token)
		{
			if(WebCache.Get(token.ToString()) != null)
				WebCache.Remove(token.ToString());
		}

		public static void SetToken(Guid token, Guid userid, string[] uts, DateTime expirationTime)
		{
			ClearToken(token);
			var t = new Token(userid, expirationTime, uts);
			WebCache.Set(token.ToString(), t, ExpirationTime);
		}

		public static void SetToken(Guid token, Token t)
		{
			SetToken(token, t.UserId,t.UserTypes, t.ExpirationTime);
		}

		public static Token GetToken(Guid token)
		{
			var t = WebCache.Get(token.ToString()) as Token;
			if (t != null)
			{
				t.ExpirationTime = DateTime.Now.AddMinutes(ExpirationTime);
				SetToken(token, t);
			}

			return t;
		}

		public static Token GetToken(string t)
		{
			Token result = null;
			Guid tk = Guid.Empty;
			if (Guid.TryParse(t, out tk) && !tk.Equals(Guid.Empty))
				result = GetToken(tk);

			return result;
		}

		public static bool CheckToken(string t)
		{
			var result = false;

			Guid tk = Guid.Empty;
			
			if (Guid.TryParse(t, out tk) && !tk.Equals(Guid.Empty))
				result = CheckToken(tk);

			return result;
		}

		public static bool CheckToken(Guid t)
		{
			var result = false;

			var theToken = GetToken(t);
			if (theToken != null && theToken.ExpirationTime >= DateTime.Now)
				result = true;

			return result;
		}
	}
}
