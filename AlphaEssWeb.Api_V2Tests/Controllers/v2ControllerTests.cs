using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using AlphaEssWeb.Api_V2.Model.ExternalResponseModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AlphaEssWeb.Api_V2.Model;
using AlphaEssWeb.Api_V2.Domain.Services;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Controllers.Tests
{
	[TestClass()]
	public class v2ControllerTests
	{
		private CryptoService _cryptoService;

		public v2ControllerTests()
		{
			_cryptoService = new Mock<CryptoService>().Object;
		}

		[TestMethod()]
		public void LoginTest()
		{
			var cryptoService = new CryptoService();

			string Username = "alpha";
			string Password = "123";
			Password = _cryptoService.EncryptNew(Password, Username);

			var model = new Externalv2LoginRequestModel
			{ Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Username = Username, Password = Password };

			//string s = GetSignForLoginUser("GTLRenewable", 1518082022, "alphaess", "ThYYVoG1KPTVp78Axopn2w==");  //sign:3013cd13a6a4c655f69e4c4856a0fe05

			model.Sign = GetSignForLoginUser(model.Api_Account, model.TimeStamp, model.Username, model.Password);

			using (var client = new HttpClient())
			{
				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/Login", model).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/Login", model).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com/ras/v2/Login", model).Result;
				var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/Login", model).Result;

				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8030/ras/v2/Login", model).Result;

				Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
				var responseObj = response.Content.ReadAsAsync<ExternalLoginResponseModel>();

				Assert.AreEqual(0, responseObj.Result.ReturnCode);
			}
		}

		private string GetToken()
		{
			var cryptoService = new CryptoService();
			var username = "alpha";
			var pwd = _cryptoService.EncryptNew("1234", username);
			var req = new Externalv2LoginRequestModel { Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Username = username, Password = pwd };
			req.Sign = GetSignForLoginUser(req.Api_Account, req.TimeStamp, username, pwd);
			var config = new HttpConfiguration();
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/Login", req).Result;
				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/Login", req).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/Login", req).Result;

				var responseObj = response.Content.ReadAsAsync<ExternalLoginResponseModel>();

				return responseObj.Result.Token;
			}
		}
		private string GetTokenForCustomer()
		{
			var cryptoService = new CryptoService();
			var username = "alphaess";
			var pwd = _cryptoService.EncryptNew("123", username);
			var req = new Externalv2LoginRequestModel { Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Username = username, Password = pwd };
			req.Sign = GetSignForLoginUser(req.Api_Account, req.TimeStamp, username, pwd);
			var config = new HttpConfiguration();
			using (var client = new HttpClient())
			{
				var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/Login", req).Result;
				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/Login", req).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/Login", req).Result;

				var responseObj = response.Content.ReadAsAsync<ExternalLoginResponseModel>();

				return responseObj.Result.Token;
			}
		}

		private string GetSignForLoginUser(string api_Account, long timeStamp, string username, string password)
		{
			var _cryptoService = new Mock<CryptoService>();

			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("username", username);
			slParams.Add("password", password);
			slParams.Add("secretkey", "ALPHAESSWEBAPI201510");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return _cryptoService.Object.GenerateMD5Hash(sbParams.ToString());
		}

		[TestMethod()]
		public void GetSystemListTest()
		{
			string token = GetToken(); //"a1794b9d-c3af-4ebd-a369-de1c6a8f0449";

			var model = new Externalv2GetSystemListRequestModel() { PageIndex = 1, PageSize = 10, Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Token = token };

			model.Sign = GetSignForSystemListTest(model.Api_Account, model.TimeStamp, model.Token, model.PageIndex, model.PageSize);

			var config = new HttpConfiguration();
			var client = new HttpClient();

			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8021/ras/v2/GetSystemList", model).Result;
			var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/GetSystemList", model).Result;
			//var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetSystemList", model).Result;

			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetSystemList", model).Result;

			Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
			var responseObj = response.Content.ReadAsAsync<ExternalSystemListResponseModel<PaginatedDto<VtSystemDto>>>();
			Assert.IsNotNull(responseObj.Result.Result);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		private string GetSignForSystemListTest(string api_Account, long timeStamp, string token, int? pageIndex, int? pageSize)
		{
			var _cryptoService = new Mock<CryptoService>();

			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Token", token);
			slParams.Add("pageindex", pageIndex);
			slParams.Add("pagesize", pageSize);
			slParams.Add("secretkey", "ALPHAESSWEBAPI201510");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return _cryptoService.Object.GenerateMD5Hash(sbParams.ToString());
		}


		[TestMethod()]
		public void EnergySummaryTest()
		{
			//string sn = "AL2001117090003";
			//string sn = "AK1407120700047";

			//string Username = "EWG10";
			//string Username = "Strickmann";  //安装商
			//string Username = "admin";  //admin

			//string Username = "AlphaService";  //servicer  all
			//string Username = "ellie";  //servicer

			//string Username = "luxi";  //reseller
			//string Username = "james";  //servicepartner

			//DateTime dt = DateTime.Parse("2017-08-19");   //DateTime.Now.ToString("d")


			string sn = "";
			//string Username = "alpha";
			string dt = "2016-06-15";

			string token = GetToken();//			"a1794b9d-c3af-4ebd-a369-de1c6a8f0449";

			var model = new Externalv2GetEnergySummaryRequestModel() { Api_Account = "android", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Sn = sn, TheDate = dt, Token = token };
			model.Sign = GetSignForEnergySummaryTest(model.Api_Account, model.TimeStamp, model.Sn, model.TheDate, model.Token);

			using (var client = new HttpClient())
			{
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8021/ras/v2/GetEnergySummary", model).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/GetEnergySummary", model).Result;
				var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetEnergySummary", model).Result;

				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetEnergySummary", model).Result;

				Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
				var responseObj = response.Content.ReadAsAsync<ExternalReportEnergyResponseModel<List<ReportEnergyDto>>>();
				Assert.AreEqual(responseObj.Result.ReturnCode, 0);
			}
		}

		private string GetSignForEnergySummaryTest(string api_Account, long timeStamp, string sn, string theDate, string token)
		{
			var _cryptoService = new Mock<CryptoService>();

			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("TheDate", theDate);
			slParams.Add("secretkey", "ALPHAESSWEBAPI201510");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return _cryptoService.Object.GenerateMD5Hash(sbParams.ToString());
		}

		[TestMethod()]
		public void GetRunningDataTest()
		{
			//string sn = "AK1505120700048";
			//string sn = "";
			//string Username = "EWG10";
			//string Username = "Strickmann";  //安装商
			//string Username = "admin";  //admin

			//string Username = "AlphaService";  //servicer  all
			//string Username = "ellie";  //servicer

			//string Username = "luxi";  //reseller
			//string Username = "james";  //servicepartner
			//string Username = "";

			string sn = "AK17A12H07000433";
			string token = "a1794b9d-c3af-4ebd-a369-de1c6a8f0449";

			var model = new Externalv2GetRunningDataRequestModel() { Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Sn = sn, Token = GetToken() };
			model.Sign = GetSignForRunningDataTest(model.Api_Account, model.TimeStamp, model.Sn, model.Token, "ALPHAESSWEBAPI201510");

			using (var client = new HttpClient())
			{
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8021/ras/v2/GetRunningData", model).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com/ras/v2/GetRunningData", model).Result;
				var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetRunningData", model).Result;

				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetRunningData", model).Result;

				Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
				var responseObj = response.Content.ReadAsAsync<ExternalVtColDataResponseModel<List<VtColDataDto>>>();
				Assert.AreEqual(responseObj.Result.ReturnCode, 0);
			}
		}

		private string GetSignForRunningDataTest(string api_Account, long timeStamp, string sn, string token, string secretkey)
		{
			var _cryptoService = new Mock<CryptoService>();

			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretkey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return _cryptoService.Object.GenerateMD5Hash(sbParams.ToString());
		}

		[TestMethod()]
		public void GetHistoryRunningDataTest()
		{
			//string sn = "AK1505120700048";
			//string sn = "AK15121207000251";
			//string Username = "EWG10";
			//string Username = "Strickmann";  //安装商
			//string Username = "admin";  //admin

			//string Username = "AlphaService";  //servicer  all
			//string Username = "ellie";  //servicer

			//string Username = "luxi";  //reseller
			//string Username = "james";  //servicepartner
			//string Username = "";
			//string Starttime = "2016-12-09 02:14:12";
			//string endtime = "2016-12-09 12:14:12";

			string token = "a1794b9d-c3af-4ebd-a369-de1c6a8f0449";

			string sn = "AK1407120700047";
			//string Username = "rs1";
			string Starttime = "2016/01/07 02:00:28";
			string endtime = "2016/01/08 02:00:28";


			var model = new Externalv2GetHistoryRunningDataRequestModel() { Api_Account = "GTLRenewable", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Sn = sn, Token = token, Starttime = Starttime, endtime = endtime };
			model.Sign = GetSignForHistoryRunningDataTest(model.Api_Account, model.TimeStamp, model.Sn, model.Token, model.Starttime, model.endtime);

			using (var client = new HttpClient())
			{
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8021/ras/v2/GetHistoryRunningData", model).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com/ras/v2/GetHistoryRunningData", model).Result;
				var response = client.PostAsJsonAsync("http://localhost:8090/v2/GetHistoryRunningData", model).Result;

				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetHistoryRunningData", model).Result;

				Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
				var responseObj = response.Content.ReadAsAsync<ExternalVtColDataResponseModel<List<VtColDataDto>>>();
				Assert.AreEqual(responseObj.Result.ReturnCode, 0);
			}
		}

		private string GetSignForHistoryRunningDataTest(string api_Account, long timeStamp, string sn, string token, string starttime, string endtime)
		{
			var _cryptoService = new Mock<CryptoService>();

			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("Starttime", starttime);
			slParams.Add("endtime", endtime);
			slParams.Add("secretkey", "GTLRenewable");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return _cryptoService.Object.GenerateMD5Hash(sbParams.ToString());
		}

		[TestMethod()]
		public void GetLastPowerDataTest()
		{
			//string sn = "AK1505120700048";
			//string sn = "AK1407120700055";
			//string Username = "EWG10";
			//string Username = "Strickmann";  //安装商
			//string Username = "admin";  //admin

			//string Username = "AlphaService";  //servicer  all
			//string Username = "ellie";  //servicer

			//string Username = "luxi";  //reseller
			//string Username = "james";  //servicepartner
			//string Username = "";


			string sn = "";
			string token = "a1794b9d-c3af-4ebd-a369-de1c6a8f0449";

			var model = new Externalv2GetLastPowerDataRequestModel() { Api_Account = "GTLRenewable", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Sn = sn, Token = token };
			model.Sign = GetSignForLastPowerDataTest(model.Api_Account, model.TimeStamp, model.Sn, model.Token);

			using (var client = new HttpClient())
			{
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8021/ras/v2/GetLastPowerData", model).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com/ras/v2/GetLastPowerData", model).Result;
				var response = client.PostAsJsonAsync("http://localhost:8090/v2/GetLastPowerData", model).Result;

				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetLastPowerData", model).Result;

				Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
				var responseObj = response.Content.ReadAsAsync<ExternalLastPowerDataResponseModel<List<PowerDataDto>>>();
				Assert.AreEqual(responseObj.Result.ReturnCode, 0);
			}
		}

		private string GetSignForLastPowerDataTest(string api_Account, long timeStamp, string sn, string token)
		{
			var _cryptoService = new Mock<CryptoService>();

			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", "GTLRenewable");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return _cryptoService.Object.GenerateMD5Hash(sbParams.ToString());
		}


		[TestMethod()]
		public void RemoteDispatchTest()
		{
			//string sn = "AK1501120700049";
			//string sn = "AK1407120700055";
			//string Username = "EWG10";
			//string Username = "Strickmann";  //安装商
			//string Username = "admin";  //admin

			//string Username = "AlphaService";  //servicer  all
			//string Username = "ellie";  //servicer

			//string Username = "luxi";  //reseller
			//string Username = "james";  //servicepartner

			string token = "a1794b9d-c3af-4ebd-a369-de1c6a8f0449";

			string sn = "AK1407120700047";

			int activePower = 20;
			int reactivePower = 20;
			decimal soc = 0.18m;
			int status = 1;
			int controlMode = 3;

			var model = new Externalv2RemoteDispatchRequestModel() { Api_Account = "GTLRenewable", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), SN = sn, Token = token, ActivePower = activePower, ReactivePower = reactivePower, SOC = soc, Status = status, ControlMode = controlMode };
			model.Sign = GetSignForRemoteDispatch(model.Api_Account, model.TimeStamp, model.SN, model.Token, model.ActivePower, model.ReactivePower, model.SOC, model.Status, model.ControlMode);

			using (var client = new HttpClient())
			{
				//var response = client.PostAsJsonAsync("http://api.alphaess.com:8021/ras/v2/RemoteDispatch", model).Result;
				//var response = client.PostAsJsonAsync("http://api.alphaess.com/ras/v2/RemoteDispatch", model).Result;
				var response = client.PostAsJsonAsync("http://localhost:8090/v2/RemoteDispatch", model).Result;

				//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/RemoteDispatch", model).Result;

				Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
				var responseObj = response.Content.ReadAsAsync<ExternalRemoteDispatchResponseModel>();
				Assert.AreEqual(responseObj.Result.ReturnCode, 0);
			}
		}

		private string GetSignForRemoteDispatch(string api_Account, long timeStamp, string sn, string token, int activePower, int reactivePower, decimal soc, int status, int controlMode)
		{
			var _cryptoService = new Mock<CryptoService>();

			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_Account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("ActivePower", activePower);
			slParams.Add("ReactivePower", reactivePower);
			slParams.Add("SOC", soc);
			slParams.Add("Status", status);
			slParams.Add("ControlMode", controlMode);
			slParams.Add("secretkey", "GTLRenewable");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return _cryptoService.Object.GenerateMD5Hash(sbParams.ToString());
		}

		private string getSignForGetPowerData(string api_account, long timeStamp, string sn, string username, string date, string token)
		{
			var _cryptoService = new Mock<CryptoService>();
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("token", token);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", string.IsNullOrEmpty(sn) ? string.Empty : sn);
			slParams.Add("date", date);
			slParams.Add("username", string.IsNullOrEmpty(username) ? string.Empty : username);
			slParams.Add("secretkey", "ALPHAESSWEBAPI201510");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.Object.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod()]
		public void GetPowerDataBySnTest()
		{
			var date = "2017-11-1";
			var sn = "AK16A12L0700037";//"AL2001017010086";
			var model = new ExternalGetPowerDataRequestModel() { Api_Account = "android", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Sn = sn, Date = date };

			string token = GetToken(); //"a1794b9d-c3af-4ebd-a369-de1c6a8f0449";
			model.Sign = getSignForGetPowerData(model.Api_Account, model.TimeStamp, model.Sn, model.Username, model.Date, token);

			var config = new HttpConfiguration();
			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/mobile/Systems/QueryPowerData", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/QueryPowerData", model).Result;
			var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/QueryPowerData", model).Result;

			Assert.Equals(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetPowerDataResponseModel>();
			Assert.Equals(0, responseObj.Result.ReturnCode);
		}

		private string getSignForGetSystemDetail(string api_account, long timeStamp, string sign, string sn, string token)
		{
			var _cryptoService = new Mock<CryptoService>();
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", "ALPHAESSWEBAPI201510");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.Object.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod()]
		public void GetSystemDetailTest()
		{
			var sn = "AK1501120700079";
			var model = new ExternalGetSystemDetailRequestModel
			{ Api_Account = "iphone", Sn = sn, TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Token = GetToken() };
			model.Sign = getSignForGetSystemDetail(model.Api_Account, model.TimeStamp, model.Sign, model.Sn, model.Token);

			var config = new HttpConfiguration();
			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/mobile/Systems/QueryPowerData", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/QueryPowerData", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/QueryPowerData", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetSystemDetail", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetSystemDetailResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		private VT_SYSTEM getSystem(string sn)
		{
			//var model = new ExternalGetSystemDetailRequestModel
			//{ Api_Account = "iphone", Sn = sn, TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Token = GetToken() };
			//model.Sign = getSignForGetSystemDetail(model.Api_Account, model.TimeStamp, model.Sign, model.Sn, model.Token);

			//var config = new HttpConfiguration();
			//var client = new HttpClient();
			////var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/mobile/Systems/QueryPowerData", model).Result;
			////var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/QueryPowerData", model).Result;
			////var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/QueryPowerData", model).Result;
			//var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetSystemDetail", model).Result;
			//var responseObj = response.Content.ReadAsAsync<ExternalGetSystemDetailResponseModel>();
			var systemSvc = new Mock<SystemService>();
			return systemSvc.Object.GetSystemBySn(sn, new Guid("84F66320-537A-4365-8B65-A7A82DC85465"));
		}

		private string getSignForUpdateSystem(string api_account, long timeStamp, string token, string secretKey, string sysid, string comid, string sn, Dictionary<string, string> editProperties)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sys_sn", string.IsNullOrEmpty(sn) ? string.Empty : sn);
			//slParams.Add("SystemId", sysid);
			//slParams.Add("CompanyId", comid);
			slParams.Add("Token", token);
			foreach (var itm in editProperties)
				slParams.Add(itm.Key, itm.Value);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}
		private Dictionary<string, string> GetVtSystemEditProperties(VT_SYSTEM orgSystem, VT_SYSTEM s)
		{
			Dictionary<string, string> slParams = new Dictionary<string, string>();

			if (orgSystem != null)
			{
				if (orgSystem.LicNo != s.LicNo)
					slParams.Add("LicNo", string.IsNullOrEmpty(s.LicNo) ? string.Empty : s.LicNo);
				if (orgSystem.Address != s.Address)
					slParams.Add("Address", string.IsNullOrEmpty(s.Address) ? string.Empty : s.Address);
				if (orgSystem.PostCode != s.PostCode)
					slParams.Add("PostCode", string.IsNullOrEmpty(s.PostCode) ? string.Empty : s.PostCode);
				if (orgSystem.Setemail != s.Setemail)
					slParams.Add("Setemail", string.IsNullOrEmpty(s.Setemail) ? string.Empty : s.Setemail);
				if (orgSystem.Linkman != s.Linkman)
					slParams.Add("Linkman", string.IsNullOrEmpty(s.Linkman) ? string.Empty : s.Linkman);
				if (orgSystem.MoneyType != s.MoneyType)
					slParams.Add("MoneyType", string.IsNullOrEmpty(s.MoneyType) ? string.Empty : s.MoneyType);
				if (orgSystem.CellPhone != s.CellPhone)
					slParams.Add("CellPhone", string.IsNullOrEmpty(s.CellPhone) ? string.Empty : s.CellPhone);
				if (orgSystem.CountryCode != s.CountryCode)
					slParams.Add("CountryCode", string.IsNullOrEmpty(s.CountryCode) ? string.Empty : s.CountryCode);
				if (orgSystem.StateCode != s.StateCode)
					slParams.Add("StateCode", string.IsNullOrEmpty(s.StateCode) ? string.Empty : s.StateCode);
				if (orgSystem.CityCode != s.CityCode)
					slParams.Add("CityCode", string.IsNullOrEmpty(s.CityCode) ? string.Empty : s.CityCode);
				if (orgSystem.Saleprice0 != s.Saleprice0)
					slParams.Add("Saleprice0", s.Saleprice0.HasValue ? s.Saleprice0.Value.ToString() : "0");
				if (orgSystem.Saleprice1 != s.Saleprice1)
					slParams.Add("Saleprice1", s.Saleprice1.HasValue ? s.Saleprice1.Value.ToString() : "0");
				if (orgSystem.Saleprice2 != s.Saleprice2)
					slParams.Add("Saleprice2", s.Saleprice2.HasValue ? s.Saleprice2.Value.ToString() : "0");
				if (orgSystem.Saleprice3 != s.Saleprice3)
					slParams.Add("Saleprice3", s.Saleprice3.HasValue ? s.Saleprice3.Value.ToString() : "0");
				if (orgSystem.Saleprice4 != s.Saleprice4)
					slParams.Add("Saleprice4", s.Saleprice4.HasValue ? s.Saleprice4.Value.ToString() : "0");
				if (orgSystem.Saleprice5 != s.Saleprice5)
					slParams.Add("Saleprice5", s.Saleprice5.HasValue ? s.Saleprice5.Value.ToString() : "0");
				if (orgSystem.Saleprice6 != s.Saleprice6)
					slParams.Add("Saleprice6", s.Saleprice6.HasValue ? s.Saleprice6.Value.ToString() : "0");
				if (orgSystem.Saleprice7 != s.Saleprice7)
					slParams.Add("Saleprice7", s.Saleprice7.HasValue ? s.Saleprice7.Value.ToString() : "0");
				if (orgSystem.SaletimeS0 != s.SaletimeS0)
					slParams.Add("SaletimeS0", s.SaletimeS0.ToString());
				if (orgSystem.SaletimeS1 != s.SaletimeS1)
					slParams.Add("SaletimeS1", s.SaletimeS1.ToString());
				if (orgSystem.SaletimeS2 != s.SaletimeS2)
					slParams.Add("SaletimeS2", s.SaletimeS2.ToString());
				if (orgSystem.SaletimeS3 != s.SaletimeS3)
					slParams.Add("SaletimeS3", s.SaletimeS3.ToString());
				if (orgSystem.SaletimeS4 != s.SaletimeS4)
					slParams.Add("SaletimeS4", s.SaletimeS4.ToString());
				if (orgSystem.SaletimeS5 != s.SaletimeS5)
					slParams.Add("SaletimeS5", s.SaletimeS5.ToString());
				if (orgSystem.SaletimeS6 != s.SaletimeS6)
					slParams.Add("SaletimeS6", s.SaletimeS6.ToString());
				if (orgSystem.SaletimeS7 != s.SaletimeS7)
					slParams.Add("SaletimeS7", s.SaletimeS7.ToString());
				if (orgSystem.SaletimeE0 != s.SaletimeE0)
					slParams.Add("SaletimeE0", s.SaletimeE0.ToString());
				if (orgSystem.SaletimeE1 != s.SaletimeE1)
					slParams.Add("SaletimeE1", s.SaletimeE1.ToString());
				if (orgSystem.SaletimeE2 != s.SaletimeE2)
					slParams.Add("SaletimeE2", s.SaletimeE2.ToString());
				if (orgSystem.SaletimeE3 != s.SaletimeE3)
					slParams.Add("SaletimeE3", s.SaletimeE3.ToString());
				if (orgSystem.SaletimeE4 != s.SaletimeE4)
					slParams.Add("SaletimeE4", s.SaletimeE4.ToString());
				if (orgSystem.SaletimeE5 != s.SaletimeE5)
					slParams.Add("SaletimeE5", s.SaletimeE5.ToString());
				if (orgSystem.SaletimeE6 != s.SaletimeE6)
					slParams.Add("SaletimeE6", s.SaletimeE6.ToString());
				if (orgSystem.SaletimeE7 != s.SaletimeE7)
					slParams.Add("SaletimeE7", s.SaletimeE7.ToString());
				if (orgSystem.Gridcharge != s.Gridcharge)
					slParams.Add("Gridcharge", s.Gridcharge.HasValue ? s.Gridcharge.Value.ToString() : "0");
				if (orgSystem.Timechaf1 != s.Timechaf1)
					slParams.Add("Timechaf1", s.Timechaf1.ToString());
				if (orgSystem.Timechae1 != s.Timechae1)
					slParams.Add("Timechae1", s.Timechae1.ToString());
				if (orgSystem.Timechaf2 != s.Timechaf2)
					slParams.Add("Timechaf2", s.Timechaf2.ToString());
				if (orgSystem.Timechae2 != s.Timechae2)
					slParams.Add("Timechae2", s.Timechae2.ToString());
				if (orgSystem.Bathighcap != s.Bathighcap)
					slParams.Add("Bathighcap", s.Bathighcap.HasValue ? s.Bathighcap.Value.ToString() : "0");
				if (orgSystem.CtrDis != s.CtrDis)
					slParams.Add("CtrDis", s.CtrDis.HasValue ? s.CtrDis.Value.ToString() : "0");
				if (orgSystem.Timedisf1 != s.Timedisf1)
					slParams.Add("Timedisf1", s.Timedisf1.ToString());
				if (orgSystem.Timedise1 != s.Timedise1)
					slParams.Add("Timedise1", s.Timedise1.ToString());
				if (orgSystem.Timedisf2 != s.Timedisf2)
					slParams.Add("Timedisf2", s.Timedisf2.ToString());
				if (orgSystem.Timedise2 != s.Timedise2)
					slParams.Add("Timedise2", s.Timedise2.ToString());
				if (orgSystem.Batusecap != s.Batusecap)
					slParams.Add("Batusecap", s.Batusecap.HasValue ? s.Batusecap.Value.ToString() : "0");
				if (orgSystem.Generator != s.Generator)
					slParams.Add("Generator", s.Generator.HasValue ? s.Generator.Value.ToString() : "0");
				if (orgSystem.GeneratorMode != s.GeneratorMode)
					slParams.Add("GeneratorMode", s.GeneratorMode.HasValue ? s.GeneratorMode.Value.ToString() : "0");
				if (orgSystem.GCSOCStart != s.GCSOCStart)
					slParams.Add("GCSOCStart", s.GCSOCStart.HasValue ? s.GCSOCStart.Value.ToString() : "0");
				if (orgSystem.GCSOCEnd != s.GCSOCEnd)
					slParams.Add("GCSOCEnd", s.GCSOCEnd.HasValue ? s.GCSOCEnd.Value.ToString() : "0");
				if (orgSystem.GCTimeStart != s.GCTimeStart)
					slParams.Add("GCTimeStart", s.GCTimeStart.HasValue ? s.GCTimeStart.Value.ToString() : "0");
				if (orgSystem.GCTimeEnd != s.GCTimeEnd)
					slParams.Add("GCTimeEnd", s.GCTimeEnd.HasValue ? s.GCTimeEnd.Value.ToString() : "0");
				if (orgSystem.GCChargePower != s.GCChargePower)
					slParams.Add("GCChargePower", s.GCChargePower.HasValue ? s.GCChargePower.Value.ToString() : "0");
				if (orgSystem.GCRatedPower != s.GCRatedPower)
					slParams.Add("GCRatedPower", s.GCRatedPower.HasValue ? s.GCRatedPower.Value.ToString() : "0");
				if (orgSystem.GCOutputMode != s.GCOutputMode)
					slParams.Add("GCOutputMode", s.GCOutputMode.HasValue ? s.GCOutputMode.Value.ToString() : "0");
				if (orgSystem.Workmode != s.Workmode)
					slParams.Add("Workmode", s.Workmode.HasValue ? s.Workmode.Value.ToString() : "0");
				if (orgSystem.AllowAutoUpdate != s.AllowAutoUpdate)
					slParams.Add("AllowAutoUpdate", s.AllowAutoUpdate.HasValue ? s.AllowAutoUpdate.Value.ToString() : "0");
				if (orgSystem.Settime != s.Settime)
					slParams.Add("Settime", string.IsNullOrEmpty(s.Settime) ? string.Empty : s.Settime);
				if (orgSystem.Acdc != s.Acdc)
					slParams.Add("Acdc", string.IsNullOrEmpty(s.Acdc) ? string.Empty : s.Acdc);
				if (orgSystem.SetFeed != s.SetFeed)
					slParams.Add("SetFeed", s.SetFeed.ToString());
				if (orgSystem.SetPhase != s.SetPhase)
					slParams.Add("SetPhase", s.SetPhase.ToString());
				if (orgSystem.Setmode != s.Setmode)
					slParams.Add("Setmode", s.Setmode.ToString());
				if (orgSystem.PowerSource != s.PowerSource)
					slParams.Add("PowerSource", string.IsNullOrEmpty(s.PowerSource) ? string.Empty : s.PowerSource);
				if (orgSystem.BackUpBox != s.BackUpBox)
					slParams.Add("BackUpBox", s.BackUpBox.HasValue ? s.BackUpBox.Value.ToString() : "0");
				if (orgSystem.L1SocLimit != s.L1SocLimit)
					slParams.Add("L1SocLimit", s.L1SocLimit.HasValue ? s.L1SocLimit.Value.ToString() : "0");
				if (orgSystem.L2SocLimit != s.L2SocLimit)
					slParams.Add("L2SocLimit", s.L2SocLimit.HasValue ? s.L2SocLimit.Value.ToString() : "0");
				if (orgSystem.L3SocLimit != s.L3SocLimit)
					slParams.Add("L3SocLimit", s.L3SocLimit.HasValue ? s.L3SocLimit.Value.ToString() : "0");
				if (orgSystem.L1Priority != s.L1Priority)
					slParams.Add("L1Priority", s.L1Priority.HasValue ? s.L1Priority.Value.ToString() : "0");
				if (orgSystem.L2Priority != s.L2Priority)
					slParams.Add("L2Priority", s.L2Priority.HasValue ? s.L2Priority.ToString() : "0");
				if (orgSystem.L3Priority != s.L3Priority)
					slParams.Add("L3Priority", s.L3Priority.HasValue ? s.L3Priority.Value.ToString() : "0");
				if (orgSystem.EmsLanguage != s.EmsLanguage)
					slParams.Add("EmsLanguage", string.IsNullOrEmpty(s.EmsLanguage) ? string.Empty : s.EmsLanguage);
				if (orgSystem.SysTimezone != s.SysTimezone)
					slParams.Add("SysTimezone", string.IsNullOrEmpty(s.SysTimezone) ? string.Empty : s.SysTimezone);
				if (orgSystem.GridType != s.GridType)
					slParams.Add("GridType", s.GridType.HasValue ? s.GridType.Value.ToString() : "0");
				if (orgSystem.FirmwareVersion != s.FirmwareVersion)
					slParams.Add("FirmwareVersion", string.IsNullOrEmpty(s.FirmwareVersion) ? string.Empty : s.FirmwareVersion);
				if (orgSystem.OFSEpvTotal != s.OFSEpvTotal)
					slParams.Add("OFSEpvTotal", s.OFSEpvTotal.HasValue ? s.OFSEpvTotal.Value.ToString() : "0");
				if (orgSystem.OFSEinput != s.OFSEinput)
					slParams.Add("OFSEinput", s.OFSEinput.HasValue ? s.OFSEinput.Value.ToString() : "0");
				if (orgSystem.OFSEoutput != s.OFSEoutput)
					slParams.Add("OFSEoutput", s.OFSEoutput.HasValue ? s.OFSEoutput.Value.ToString() : "0");
				if (orgSystem.OFSEcharge != s.OFSEcharge)
					slParams.Add("OFSEcharge", s.OFSEcharge.HasValue ? s.OFSEcharge.Value.ToString() : "0");
				if (orgSystem.OFSEGridCharge != s.OFSEGridCharge)
					slParams.Add("OFSEGridCharge", s.OFSEGridCharge.HasValue ? s.OFSEGridCharge.Value.ToString() : "0");
				if (orgSystem.OFSEdischarge != s.OFSEdischarge)
					slParams.Add("OFSEdischarge", s.OFSEdischarge.HasValue ? s.OFSEdischarge.Value.ToString() : "0");
				if (orgSystem.OnGridCap != s.OnGridCap)
					slParams.Add("OnGridCap", s.OnGridCap.HasValue ? s.OnGridCap.Value.ToString() : "0");
				if (orgSystem.StorageCap != s.StorageCap)
					slParams.Add("StorageCap", s.StorageCap.HasValue ? s.StorageCap.Value.ToString("") : "0");
				if (orgSystem.BatReady != s.BatReady)
					slParams.Add("BatReady", string.IsNullOrEmpty(s.BatReady) ? string.Empty : s.BatReady);
				if (orgSystem.MeterDCNegate != s.MeterDCNegate)
					slParams.Add("MeterDCNegate", string.IsNullOrEmpty(s.MeterDCNegate) ? string.Empty : s.MeterDCNegate);
				if (orgSystem.MeterACNegate != s.MeterACNegate)
					slParams.Add("MerterACNegate", string.IsNullOrEmpty(s.MeterACNegate) ? string.Empty : s.MeterACNegate);
				if (orgSystem.Safe != s.Safe)
					slParams.Add("Safe", s.Safe.HasValue ? s.Safe.Value.ToString("") : "0");
				if (orgSystem.PowerFact != s.PowerFact)
					slParams.Add("PowerFact", s.PowerFact.HasValue ? s.PowerFact.Value.ToString("") : "0");
				if (orgSystem.Volt5MinAvg != s.Volt5MinAvg)
					slParams.Add("Volt5MinAvg", s.Volt5MinAvg.HasValue ? s.Volt5MinAvg.Value.ToString("") : "0");
				if (orgSystem.Volt10MinAvg != s.Volt10MinAvg)
					slParams.Add("Volt10MinAvg", s.Volt10MinAvg.HasValue ? s.Volt10MinAvg.Value.ToString("") : "0");
				if (orgSystem.TempThreshold != s.TempThreshold)
					slParams.Add("TempThreshold", s.TempThreshold.HasValue ? s.TempThreshold.Value.ToString("") : "0");
				if (orgSystem.OutCurProtect != s.OutCurProtect)
					slParams.Add("OutCurProtect", s.OutCurProtect.ToString());
				if (orgSystem.DCI != s.DCI)
					slParams.Add("DCI", s.DCI.HasValue ? s.DCI.Value.ToString() : "0");
				if (orgSystem.RCD != s.RCD)
					slParams.Add("RCD", s.RCD.HasValue ? s.RCD.Value.ToString("") : "0");
				if (orgSystem.PvISO != s.PvISO)
					slParams.Add("PvISO", s.PvISO.HasValue ? s.PvISO.Value.ToString("") : "0");
				if (orgSystem.ChargeBoostCur != s.ChargeBoostCur)
					slParams.Add("ChargeBoostCur", s.ChargeBoostCur.HasValue ? s.ChargeBoostCur.Value.ToString() : "0");
				if (orgSystem.Channel1 != s.Channel1)
					slParams.Add("Channel1", string.IsNullOrEmpty(s.Channel1) ? string.Empty : s.Channel1);
				if (orgSystem.ControlMode1 != s.ControlMode1)
					slParams.Add("ControlMode1", string.IsNullOrEmpty(s.ControlMode1) ? string.Empty : s.ControlMode1);
				if (orgSystem.StartTime1A != s.StartTime1A)
					slParams.Add("StartTime1A", string.IsNullOrEmpty(s.StartTime1A) ? string.Empty : s.StartTime1A);
				if (orgSystem.EndTime1A != s.EndTime1A)
					slParams.Add("EndTime1A", string.IsNullOrEmpty(s.EndTime1A) ? string.Empty : s.EndTime1A);
				if (orgSystem.StartTime1B != s.StartTime1B)
					slParams.Add("StartTime1B", string.IsNullOrEmpty(s.StartTime1B) ? string.Empty : s.StartTime1B);
				if (orgSystem.EndTime1B != s.EndTime1B)
					slParams.Add("EndTime1B", string.IsNullOrEmpty(s.EndTime1B) ? string.Empty : s.EndTime1B);
				if (orgSystem.Date1 != s.Date1)
					slParams.Add("Date1", string.IsNullOrEmpty(s.Date1) ? string.Empty : s.Date1);
				if (orgSystem.ChargeSOC1 != s.ChargeSOC1)
					slParams.Add("ChargeSOC1", string.IsNullOrEmpty(s.ChargeSOC1) ? string.Empty : s.ChargeSOC1);
				if (orgSystem.UPS1 != s.UPS1)
					slParams.Add("UPS1", string.IsNullOrEmpty(s.UPS1) ? string.Empty : s.UPS1);
				if (orgSystem.SwitchOn1 != s.SwitchOn1)
					slParams.Add("SwitchOn1", s.SwitchOn1.HasValue ? s.SwitchOn1.Value.ToString("") : "0");
				if (orgSystem.SwitchOff1 != s.SwitchOff1)
					slParams.Add("SwitchOff1", s.SwitchOff1.HasValue ? s.SwitchOff1.Value.ToString("") : "0");
				if (orgSystem.Delay1 != s.Delay1)
					slParams.Add("Delay1", string.IsNullOrEmpty(s.Delay1) ? string.Empty : s.Delay1);
				if (orgSystem.Duration1 != s.Duration1)
					slParams.Add("Duration1", string.IsNullOrEmpty(s.Duration1) ? string.Empty : s.Duration1);
				if (orgSystem.Pause1 != s.Pause1)
					slParams.Add("Pause1", string.IsNullOrEmpty(s.Pause1) ? string.Empty : s.Pause1);
				if (orgSystem.Channel2 != s.Channel2)
					slParams.Add("Channel2", string.IsNullOrEmpty(s.Channel2) ? string.Empty : s.Channel2);
				if (orgSystem.ControlMode2 != s.ControlMode2)
					slParams.Add("ControlMode2", string.IsNullOrEmpty(s.ControlMode2) ? string.Empty : s.ControlMode2);
				if (orgSystem.StartTime2A != s.StartTime2A)
					slParams.Add("StartTime2A", string.IsNullOrEmpty(s.StartTime2A) ? string.Empty : s.StartTime2A);
				if (orgSystem.EndTime2A != s.EndTime2A)
					slParams.Add("EndTime2A", string.IsNullOrEmpty(s.EndTime2A) ? string.Empty : s.EndTime2A);
				if (orgSystem.StartTime2B != s.StartTime2B)
					slParams.Add("StartTime2B", string.IsNullOrEmpty(s.StartTime2B) ? string.Empty : s.StartTime2B);
				if (orgSystem.EndTime2B != s.EndTime2B)
					slParams.Add("EndTime2B", string.IsNullOrEmpty(s.EndTime2B) ? string.Empty : s.EndTime2B);
				if (orgSystem.Date2 != s.Date2)
					slParams.Add("Date2", string.IsNullOrEmpty(s.Date2) ? string.Empty : s.Date2);
				if (orgSystem.ChargeSOC2 != s.ChargeSOC2)
					slParams.Add("ChargeSOC2", string.IsNullOrEmpty(s.ChargeSOC2) ? string.Empty : s.ChargeSOC2);
				if (orgSystem.UPS2 != s.UPS2)
					slParams.Add("UPS2", string.IsNullOrEmpty(s.UPS2) ? string.Empty : s.UPS2);
				if (orgSystem.SwitchOn2 != s.SwitchOn2)
					slParams.Add("SwitchOn2", s.SwitchOn2.HasValue ? s.SwitchOn2.Value.ToString("") : "0");
				if (orgSystem.SwitchOff2 != s.SwitchOff2)
					slParams.Add("SwitchOff2", s.SwitchOff2.HasValue ? s.SwitchOff2.Value.ToString("") : "0");
				if (orgSystem.Delay2 != s.Delay2)
					slParams.Add("Delay2", string.IsNullOrEmpty(s.Delay2) ? string.Empty : s.Delay2);
				if (orgSystem.Duration2 != s.Duration2)
					slParams.Add("Duration2", string.IsNullOrEmpty(s.Duration2) ? string.Empty : s.Duration2);
				if (orgSystem.Pause2 != s.Pause2)
					slParams.Add("Pause2", string.IsNullOrEmpty(s.Pause2) ? string.Empty : s.Pause2);
				if (orgSystem.AllowSyncTime != s.AllowSyncTime)
					slParams.Add("AllowSyncTime", s.AllowSyncTime.HasValue ? s.AllowSyncTime.Value.ToString() : "0");
				if (orgSystem.BakBoxSN != s.BakBoxSN)
					slParams.Add("BakBoxSN", string.IsNullOrEmpty(s.BakBoxSN) ? string.Empty : s.BakBoxSN);
				if (orgSystem.BakBoxVer != s.BakBoxVer)
					slParams.Add("BakBoxVer", string.IsNullOrEmpty(s.BakBoxVer) ? string.Empty : s.BakBoxVer);
				if (orgSystem.Batterysn1 != s.Batterysn1)
					slParams.Add("Batterysn1", string.IsNullOrEmpty(s.Batterysn1) ? string.Empty : s.Batterysn1);
				if (orgSystem.Batterysn10 != s.Batterysn10)
					slParams.Add("Batterysn10", string.IsNullOrEmpty(s.Batterysn10) ? string.Empty : s.Batterysn10);
				if (orgSystem.Batterysn11 != s.Batterysn11)
					slParams.Add("Batterysn11", string.IsNullOrEmpty(s.Batterysn11) ? string.Empty : s.Batterysn11);
				if (orgSystem.Batterysn12 != s.Batterysn12)
					slParams.Add("Batterysn12", string.IsNullOrEmpty(s.Batterysn12) ? string.Empty : s.Batterysn12);
				if (orgSystem.Batterysn13 != s.Batterysn13)
					slParams.Add("Batterysn13", string.IsNullOrEmpty(s.Batterysn13) ? string.Empty : s.Batterysn13);
				if (orgSystem.Batterysn14 != s.Batterysn14)
					slParams.Add("Batterysn14", string.IsNullOrEmpty(s.Batterysn14) ? string.Empty : s.Batterysn14);
				if (orgSystem.Batterysn15 != s.Batterysn15)
					slParams.Add("Batterysn15", string.IsNullOrEmpty(s.Batterysn15) ? string.Empty : s.Batterysn15);
				if (orgSystem.Batterysn16 != s.Batterysn16)
					slParams.Add("Batterysn16", string.IsNullOrEmpty(s.Batterysn16) ? string.Empty : s.Batterysn16);
				if (orgSystem.Batterysn17 != s.Batterysn17)
					slParams.Add("Batterysn17", string.IsNullOrEmpty(s.Batterysn17) ? string.Empty : s.Batterysn17);
				if (orgSystem.Batterysn18 != s.Batterysn18)
					slParams.Add("Batterysn18", string.IsNullOrEmpty(s.Batterysn18) ? string.Empty : s.Batterysn18);
				if (orgSystem.Batterysn2 != s.Batterysn2)
					slParams.Add("Batterysn2", string.IsNullOrEmpty(s.Batterysn2) ? string.Empty : s.Batterysn2);
				if (orgSystem.Batterysn3 != s.Batterysn3)
					slParams.Add("Batterysn3", string.IsNullOrEmpty(s.Batterysn3) ? string.Empty : s.Batterysn3);
				if (orgSystem.Batterysn4 != s.Batterysn4)
					slParams.Add("Batterysn4", string.IsNullOrEmpty(s.Batterysn4) ? string.Empty : s.Batterysn4);
				if (orgSystem.Batterysn5 != s.Batterysn5)
					slParams.Add("Batterysn5", string.IsNullOrEmpty(s.Batterysn5) ? string.Empty : s.Batterysn5);
				if (orgSystem.Batterysn6 != s.Batterysn6)
					slParams.Add("Batterysn6", string.IsNullOrEmpty(s.Batterysn6) ? string.Empty : s.Batterysn6);
				if (orgSystem.Batterysn7 != s.Batterysn7)
					slParams.Add("Batterysn7", string.IsNullOrEmpty(s.Batterysn7) ? string.Empty : s.Batterysn7);
				if (orgSystem.Batterysn8 != s.Batterysn8)
					slParams.Add("Batterysn8", string.IsNullOrEmpty(s.Batterysn8) ? string.Empty : s.Batterysn8);
				if (orgSystem.Batterysn9 != s.Batterysn9)
					slParams.Add("Batterysn9", string.IsNullOrEmpty(s.Batterysn9) ? string.Empty : s.Batterysn9);
				if (orgSystem.Bmsversion != s.Bmsversion)
					slParams.Add("Bmsversion", string.IsNullOrEmpty(s.Bmsversion) ? string.Empty : s.Bmsversion);
				if (orgSystem.BMUModel != s.BMUModel)
					slParams.Add("BMUModel", string.IsNullOrEmpty(s.BMUModel) ? string.Empty : s.BMUModel);
				if (orgSystem.CheckTime != s.CheckTime)
					slParams.Add("CheckTime", s.CheckTime.HasValue ? s.CheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
				if (orgSystem.Cobat != s.Cobat)
					slParams.Add("Cobat", s.Cobat.HasValue ? s.Cobat.Value.ToString() : "0");
				if (orgSystem.CompanyId != s.CompanyId)
					slParams.Add("CompanyId", s.CompanyId.HasValue ? s.CompanyId.Value.ToString() : string.Empty);
				if (orgSystem.CTRate != s.CTRate)
					slParams.Add("CTRate", s.CTRate.HasValue ? s.CTRate.Value.ToString() : "0");
				if (orgSystem.DeleteFlag != s.DeleteFlag)
					slParams.Add("DeleteFlag", s.DeleteFlag.HasValue ? s.DeleteFlag.Value.ToString() : "0");
				if (orgSystem.Fan != s.Fan)
					slParams.Add("Fan", s.Fan.HasValue ? s.Fan.Value.ToString() : "0");
				if (orgSystem.Fax != s.Fax)
					slParams.Add("Fax", string.IsNullOrEmpty(s.Fax) ? string.Empty : s.Fax);
				if (orgSystem.Emsversion != s.Emsversion)
					slParams.Add("Emsversion", string.IsNullOrEmpty(s.Emsversion) ? string.Empty : s.Emsversion);
				if (orgSystem.Inputcost != s.Inputcost)
					slParams.Add("Inputcost", s.Inputcost.HasValue ? s.Inputcost.Value.ToString() : "0");
				if (orgSystem.InventerSn != s.InventerSn)
					slParams.Add("InventerSn", string.IsNullOrEmpty(s.InventerSn) ? string.Empty : s.InventerSn);
				if (orgSystem.Latitude != s.Latitude)
					slParams.Add("Latitude", string.IsNullOrEmpty(s.Latitude) ? string.Empty : s.Latitude);
				if (orgSystem.Longitude != s.Longitude)
					slParams.Add("Longitude", string.IsNullOrEmpty(s.Longitude) ? string.Empty : s.Longitude);
				if (orgSystem.Mbat != s.Mbat)
					slParams.Add("Mbat", string.IsNullOrEmpty(s.Mbat) ? string.Empty : s.Mbat);
				if (orgSystem.GridChargeWE != s.GridChargeWE)
					slParams.Add("GridChargeWE", s.GridChargeWE.HasValue ? s.GridChargeWE.Value.ToString("") : "0");
				if (orgSystem.TimeChaFWE1 != s.TimeChaFWE1)
					slParams.Add("TimeChaFWE1", s.TimeChaFWE1.HasValue ? s.TimeChaFWE1.Value.ToString("") : "0");
				if (orgSystem.TimeChaEWE1 != s.TimeChaEWE1)
					slParams.Add("TimeChaEWE1", s.TimeChaEWE1.HasValue ? s.TimeChaEWE1.Value.ToString("") : "0");
				if (orgSystem.TimeChaFWE2 != s.TimeChaFWE2)
					slParams.Add("TimeChaFWE2", s.TimeChaFWE2.HasValue ? s.TimeChaFWE2.Value.ToString("") : "0");
				if (orgSystem.TimeChaEWE2 != s.TimeChaEWE2)
					slParams.Add("TimeChaEWE2", s.TimeChaEWE2.HasValue ? s.TimeChaEWE2.Value.ToString("") : "0");
				if (orgSystem.TimeDisFWE1 != s.TimeDisFWE1)
					slParams.Add("TimeDisFWE1", s.TimeDisFWE1.HasValue ? s.TimeDisFWE1.Value.ToString("") : "0");
				if (orgSystem.TimeDisEWE1 != s.TimeDisEWE1)
					slParams.Add("TimeDisEWE1", s.TimeDisEWE1.HasValue ? s.TimeDisEWE1.Value.ToString("") : "0");
				if (orgSystem.TimeDisFWE2 != s.TimeDisFWE2)
					slParams.Add("TimeDisFWE2", s.TimeDisFWE2.HasValue ? s.TimeDisFWE2.Value.ToString("") : "0");
				if (orgSystem.TimeDisEWE2 != s.TimeDisEWE2)
					slParams.Add("TimeDisEWE2", s.TimeDisEWE2.HasValue ? s.TimeDisEWE2.Value.ToString("") : "0");
				if (orgSystem.BatHighCapWE != s.BatHighCapWE)
					slParams.Add("BatHighCapWE", s.BatHighCapWE.HasValue ? s.BatHighCapWE.Value.ToString("") : "0");
				if (orgSystem.BatUseCapWE != s.BatUseCapWE)
					slParams.Add("BatUseCapWE", s.BatUseCapWE.HasValue ? s.BatUseCapWE.Value.ToString("") : "0");
				if (orgSystem.CtrDisWE != s.CtrDisWE)
					slParams.Add("CtrDisWE", s.CtrDisWE.HasValue ? s.CtrDisWE.Value.ToString("") : "0");
				if (orgSystem.ChargeWorkDays != s.ChargeWorkDays)
					slParams.Add("ChargeWorkDays", string.IsNullOrEmpty(s.ChargeWorkDays) ? string.Empty : s.ChargeWorkDays);
				if (orgSystem.ChargeWeekend != s.ChargeWeekend)
					slParams.Add("ChargeWeekend", string.IsNullOrEmpty(s.ChargeWeekend) ? string.Empty : s.ChargeWeekend);
			}

			return slParams;
		}

		[TestMethod()]
		public void getSignForUpdateSystemTest()
		{
			var dicProperties = new Dictionary<string, string>();
			dicProperties.Add("StartTime1A", "01:00");
			dicProperties.Add("ControlMode1", "Off");
			var result = getSignForUpdateSystem("android", 1529571895, "328ba592-5ccf-45f1-b430-8da2e687b635", "ALPHAESSWEBAPI201510", "f63b627c-006e-4cc7-8864-75468f38a2ae", "84f66320-537a-4365-8b65-a7a82dc85465", "AK18116F07000743", dicProperties);
			Assert.AreEqual("40ada873bf9bd93a58a8da00e853855c", result);
		}
		[TestMethod()]
		public void SetSystemTest()
		{
			var sn = "AK1501120700079";
			var account = "iphone";
			var token = GetToken();
			var timestamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
			var ACDC = "DC";
			//var cellphone = "1231231231";
			//var postcode = "3333";
			//var channel1 = "0";
			//var startTime1A = "01:00";
			//var OnGridCap = 8;
			//var Popv = 7;
			//var PowerFact = 0;
			//var SetFeed = 70;
			//var SysTimezone = "China Standard Time";
			//var EndTime1B = "4:00";
			//var Date1 = "0101010";
			//var ChargeModel1 = 3;
			//var ChargeSOC1 = "20";
			//var UPS1 = "1";
			//var SwitchOn1 = 1000;
			//var SwitchOff1 = 200;
			//var Delay1 = 5;
			//var Duration1 = 10;
			//var Pause1 = 10;
			var dicProperties = new Dictionary<string, string>();
			//dicProperties.Add("cellphone", cellphone);
			//dicProperties.Add("postcode", postcode);
			//dicProperties.Add("Channel1", channel1);
			dicProperties.Add("ACDC", ACDC);
			//dicProperties.Add("OnGridCap", OnGridCap.ToString());
			//dicProperties.Add("Popv", Popv.ToString());
			//dicProperties.Add("PowerFact", PowerFact.ToString());
			//dicProperties.Add("SetFeed", SetFeed.ToString());
			//dicProperties.Add("SysTimezone", SysTimezone);
			//dicProperties.Add("Date1", Date1);
			//dicProperties.Add("ChargeSOC1", ChargeSOC1);
			//dicProperties.Add("UPS1", UPS1);
			//dicProperties.Add("ChargeModel1", ChargeModel1.ToString());
			//dicProperties.Add("SwitchOn1", SwitchOn1.ToString());
			//dicProperties.Add("SwitchOff1", SwitchOff1.ToString());
			//dicProperties.Add("Delay1", Delay1.ToString());
			//dicProperties.Add("Duration1", Duration1.ToString());
			//dicProperties.Add("Pause1", Pause1.ToString());
			//dicProperties.Add("TimeChaFWE1", timeChaFWE1.ToString());
			//dicProperties.Add("TimeChaEWE1", timeChaEWE1.ToString());
			//dicProperties.Add("TimeChaFWE2", timeChaFWE2.ToString());
			//dicProperties.Add("TimeChaEWE2", timeChaEWE2.ToString());
			//dicProperties.Add("TimeDisFWE1", timeDisFWE1.ToString());
			//dicProperties.Add("TimeDisEWE1", timeDisEWE1.ToString());
			//dicProperties.Add("TimeDisFWE2", timeDisFWE2.ToString());
			//dicProperties.Add("TimeDisEWE2", timeDisEWE2.ToString());
			var sign = getSignForUpdateSystem(account, timestamp, token, "ALPHAESSWEBAPI201510", string.Empty, string.Empty, sn, dicProperties);
			var model = new
			{
				Sys_Sn = sn, Token = token, Sign = sign, Api_Account = "iphone", TimeStamp = timestamp,
				ACDC = ACDC
			};

			var config = new HttpConfiguration();
			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/mobile/Systems/QueryPowerData", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/QueryPowerData", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/QueryPowerData", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/UpdateSystemConfig", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/UpdateSystemConfig", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetSystemDetailResponseModel>();
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		private string getSignForGetEnergeData(string api_account, long timeStamp, string sn, string username, string start, string end, string statisticsby, string token)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", string.IsNullOrEmpty(sn) ? string.Empty : sn);
			slParams.Add("Token", token);
			slParams.Add("statisticsby", string.IsNullOrEmpty(statisticsby) ? string.Empty : statisticsby);
			slParams.Add("date_start", start);
			slParams.Add("date_end", end);
			slParams.Add("username", string.IsNullOrEmpty(username) ? string.Empty : username);
			slParams.Add("secretkey", "ALPHAESSWEBAPI201510");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private string getSignForGetProfitData(string api_account, long timeStamp, string sn, string username, string start, string end, string statisticsby, string token)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sn", string.IsNullOrEmpty(sn) ? string.Empty : sn);
			slParams.Add("Token", token);
			slParams.Add("statisticsby", string.IsNullOrEmpty(statisticsby) ? string.Empty : statisticsby);
			slParams.Add("date_start", start);
			slParams.Add("date_end", end);
			slParams.Add("username", string.IsNullOrEmpty(username) ? string.Empty : username);
			slParams.Add("secretkey", "ALPHAESSWEBAPI201510");

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod()]
		public void GetEnergeDataTest()
		{
			var sn = "AK1501120700079";
			string starttime = "2018/01/07";
			string endtime = "2018/05/08";
			var model = new ExternalGetEnergeDataRequestModel
			{ Api_Account = "iphone", Sn = sn, TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), StatisticsBy = "month", Token = GetToken(), Date_Start = starttime, Date_End = endtime };
			model.Sign = getSignForGetEnergeData(model.Api_Account, model.TimeStamp, sn, string.Empty, starttime, endtime, "month", model.Token);

			var config = new HttpConfiguration();
			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/mobile/Systems/GetEnergeData", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/GetEnergeData", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetEnergeData", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetEnergeData", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetEnergeDataResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		[TestMethod()]
		public void GetProfitDataTest()
		{
			var sn = string.Empty;//"AK1501120700079";
			string starttime = "2018/01/07";
			string endtime = "2018/05/08";
			var model = new ExternalGetProfitDataRequestModel
			{ Username = "demo", Api_Account = "iphone", Sn = sn, TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), StatisticsBy = "month", Token = GetToken(), Date_Start = starttime, Date_End = endtime };
			model.Sign = getSignForGetProfitData(model.Api_Account, model.TimeStamp, sn, model.Username, starttime, endtime, "month", model.Token);

			var config = new HttpConfiguration();
			var client = new HttpClient();
			var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/GetProfitData", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/GetProfitData", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetProfitData", model).Result;
			//var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetProfitData", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetProfitDataResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		private string GetSignForRetrievePwd(string api_account, long timeStamp, string username, string email, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("username", username);
			slParams.Add("email", email);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private string getSignForEvaluateComplaints(string api_account, long timeStamp, string token, long cpid, int satsf, int satsf1, int satsf2, string content, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("complaintsId", cpid.ToString());
			slParams.Add("satisfaction", satsf.ToString());
			slParams.Add("satisfaction1", satsf1.ToString());
			slParams.Add("satisfaction2", satsf2.ToString());
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod()]
		public void RetrievePwdTest()
		{
			var model = new ExternalRetrievePwdRequestModel { Api_Account = "iphone", Email = "walker.ling@alpha-ess.com", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Username = "AlphaESS" };
			model.Sign = GetSignForRetrievePwd(model.Api_Account, model.TimeStamp, model.Username, model.Email, "ALPHAESSWEBAPI201510");

			var client = new HttpClient();
			var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/RetrievePwd", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/RetrievePwd", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/RetrievePwd", model).Result;
			//var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/RetrievePwd", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalRetrievePwdResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		private string GetSignForAddNewComplaints(string api_account, long timeStamp, string token, string title, string description, string complaintsType, string email, string contactNumber, string sn, string attachment1, string attachment2, string attachment3, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sysSn", sn);
			slParams.Add("title", title);
			slParams.Add("description", description);
			slParams.Add("attachment1", string.IsNullOrWhiteSpace(attachment1) ? string.Empty : attachment1);
			slParams.Add("attachment2", string.IsNullOrWhiteSpace(attachment2) ? string.Empty : attachment2);
			slParams.Add("attachment3", string.IsNullOrWhiteSpace(attachment3) ? string.Empty : attachment3);
			slParams.Add("complaintsType", complaintsType);
			slParams.Add("email", string.IsNullOrWhiteSpace(email) ? string.Empty : email);
			slParams.Add("contactNumber", string.IsNullOrWhiteSpace(contactNumber) ? string.Empty : contactNumber);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}
		private string getSignForGetComplaintsList(string api_account, long timeStamp, string token, int pi, int ps, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("pageindex", pi.ToString());
			slParams.Add("pagesize", ps.ToString());
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod()]
		public void NewComplaintsTest()
		{
			var model = new ExternalAddNewComplaintRequestModel
			{
				Api_Account = "iphone", Email = "walker.ling@alpha-ess.com", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), ComplaintsType = "title_inverter", ContactNumber = "1111", Description = "测试换行符\n\n", SysSn = "AK17A12H07000433",
				Title = "test0003", Attachment1 = System.Web.HttpUtility.UrlEncode("~/App_Data/201808\\Screenshot_2018-03-30-14-30-43-137.png")
			};
			model.Token = GetToken();
			model.Sign = GetSignForAddNewComplaints(model.Api_Account, model.TimeStamp, model.Token, model.Title, model.Description, model.ComplaintsType, model.Email, model.ContactNumber, model.SysSn, model.Attachment1, model.Attachment2, model.Attachment3, "ALPHAESSWEBAPI201510");

			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/RetrievePwd", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/RetrievePwd", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/RetrievePwd", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/AddNewComplaints", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalAddNewComplaintsResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		[TestMethod()]
		public void GetComplaintsListTest()
		{
			var model = new ExternalGetComplaintsListRequestModel { Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), PageIndex = 1, PageSize = 20 };
			model.Token = GetTokenForCustomer();
			model.Sign = getSignForGetComplaintsList(model.Api_Account, model.TimeStamp, model.Token, model.PageIndex, model.PageSize, "ALPHAESSWEBAPI201510");

			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/GetComplaintsList", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/GetComplaintsList", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetComplaintsList", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetComplaintsList", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetComplaintsListResponseModel<PaginatedDto<ComplaintsDto>>>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
			Assert.IsNotNull(responseObj.Result.Result.Items);
		}

		[TestMethod]
		public void EvaluateComplaintsTest()
		{
			var model = new ExternalEvaluateComplaintsRequestModel
			{
				Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Content = "test", Satisfaction = 3, Satisfaction1 = 2, Satisfaction2 = 4,
				ComplaintsId = 40818
			};
			model.Token = GetToken();
			model.Sign = getSignForEvaluateComplaints(model.Api_Account, model.TimeStamp, model.Token, model.ComplaintsId, model.Satisfaction, model.Satisfaction1, model.Satisfaction2, model.Content, "ALPHAESSWEBAPI201510");

			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/EvaluateComplaints", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/EvaluateComplaints", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/EvaluateComplaints", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/EvaluateComplaints", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalEvaluateComplaintsResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);

		}

		private string getSignForInstallNewSystem(string api_account, long timeStamp, string token, string api_SecretKey, string sn, string licNo, DateTime installationDate, string checkcode, string customerName, string contactNumber, string contactAddress)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("newSN", sn);
			slParams.Add("checkCode", checkcode);
			slParams.Add("installationDate", installationDate.ToString("yyyy-MM-dd HH:mm:ss"));
			slParams.Add("license_no", licNo);
			slParams.Add("customerName", customerName);
			slParams.Add("contactNumber", contactNumber);
			slParams.Add("contactAddress", contactAddress);
			slParams.Add("Token", token);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod]
		public void InstallNewSystemTest()
		{
			var model = new ExternalInstallNewSystemRequestModel
			{
				Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), CheckCode = "1234", ContactAddress = "asdasfewf", CustomerName = "lq", InstallationDate = DateTime.Parse("2018-8-13 00:00:00"), ContactNumber = "321231", NewSN = "testsn0009", License_no = "ALPHA0001123456"
			};
			model.Token = GetToken();
			model.Sign = getSignForInstallNewSystem(model.Api_Account, model.TimeStamp, model.Token, "ALPHAESSWEBAPI201510", model.NewSN, model.License_no, model.InstallationDate, model.CheckCode, model.CustomerName, model.ContactNumber, model.ContactAddress);

			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/InstallNewSystem", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/InstallNewSystem", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/InstallNewSystem", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/InstallNewSystem", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalInstallNewSystemResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		private string getSignForGetMsg(string api_account, long timeStamp, string token, int onlyUnread, int pi, int ps, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("pageindex", pi.ToString());
			slParams.Add("pagesize", ps.ToString());
			slParams.Add("onlyUnread", onlyUnread.ToString());
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod]
		public void GetMsgTest()
		{
			var model = new ExternalGetMsgReauestModel
			{
				Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), OnlyUnread=0, PageIndex=1, PageSize=10
			};
			model.Token = GetToken();
			model.Sign = getSignForGetMsg(model.Api_Account, model.TimeStamp, model.Token, model.OnlyUnread, model.PageIndex, model.PageSize, "ALPHAESSWEBAPI201510");

			var client = new HttpClient();
			var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/GetMsgList", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/GetMsgList", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetMsgList", model).Result;
			//var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetMsgList", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetMsgResponseModel<PaginatedDto<SysMsgDto>>>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}
		private string getSignForGetFirmwareUpdate(string api_account, long timeStamp, string token, string api_SecretKey, string sn)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Sn", sn);
			slParams.Add("Token", token);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod]
		public void GetFirmwareUpdateTest()
		{
			var model = new ExternalGetFirmwareUpdateRequestModel
			{
				Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), Sn = "AK1501120700079"
			};
			model.Token = GetToken();
			model.Sign = getSignForGetFirmwareUpdate(model.Api_Account, model.TimeStamp, model.Token, "ALPHAESSWEBAPI201510", model.Sn);

			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/GetFirmwareUpdate", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/GetFirmwareUpdate", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetFirmwareUpdate", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetFirmwareUpdate", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetFirmwareUpdateResponseModel<FirmwareVersionData>>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		private string getSignForUpdateMsgFlag(string api_account, long timeStamp, string token, int flag, string msgId, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("flag", flag.ToString());
			slParams.Add("msgId", msgId);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod]
		public void UpdateMsgFlagTest()
		{
			var model = new ExternalUpdateMsgFlagRequestModel
			{
				Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), MsgId = "BF2976F6-2CEF-4F99-8F6D-5E507B743DCF", Flag = 1
			};
			model.Token = GetToken();
			model.Sign = getSignForUpdateMsgFlag(model.Api_Account, model.TimeStamp, model.Token, model.Flag, model.MsgId, "ALPHAESSWEBAPI201510");

			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/UpdateMsgFlag", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/UpdateMsgFlag", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/UpdateMsgFlag", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/UpdateMsgFlag", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalUpdateMsgFlagResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}
		private string getSignForGetSystemSummaryStatisticsData(string api_account, long timeStamp, string token, string api_SecretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("Token", token);
			slParams.Add("secretkey", api_SecretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		[TestMethod]
		public void GetSystemSummaryStatisticsDataTest()
		{
			var model = new ExternalGetSystemSummaryStatisticsDataRequestModel
			{
				Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds), 
			};
			model.Token = GetToken();
			model.Sign = getSignForGetSystemSummaryStatisticsData(model.Api_Account, model.TimeStamp, model.Token, "ALPHAESSWEBAPI201510");

			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/GetSystemSummaryStatisticsData", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/GetSystemSummaryStatisticsData", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetSystemSummaryStatisticsData", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetSystemSummaryStatisticsData", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetSystemSummaryStatisticsDataResponsModel<SystemSummaryStatisticsData>>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
		}

		private string getSignForGetCompanyContacts(string api_account, long timeStamp, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return (_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}
		[TestMethod]
		public void GetCompanyContactsTest()
		{
			var model = new ExternalGetCompanyContactsRequestModel
			{
				Api_Account = "iphone", TimeStamp = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds),
			};
			model.Sign = getSignForGetCompanyContacts(model.Api_Account, model.TimeStamp, "ALPHAESSWEBAPI201510");

			var client = new HttpClient();
			//var response = client.PostAsJsonAsync("http://api.alphaess.com:8030/ras/v2/GetCompanyContacts", model).Result;
			//var response = client.PostAsJsonAsync("http://api.alphaess.com/mobile/Systems/GetCompanyContacts", model).Result;
			//var response = client.PostAsJsonAsync("http://dev.alphaess.com:8090/ras/v2/GetCompanyContacts", model).Result;
			var response = client.PostAsJsonAsync("http://localhost:8090/ras/v2/GetCompanyContacts", model).Result;

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var responseObj = response.Content.ReadAsAsync<ExternalGetCompanyContactsResponseModel>();
			Assert.IsNotNull(responseObj);
			Assert.AreEqual(0, responseObj.Result.ReturnCode);
			Assert.AreEqual(4, responseObj.Result.Result.Count());
		}
	}
}