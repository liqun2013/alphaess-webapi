using RemotingSocketServer;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace AlphaEss.Api_V2.Infrastructure
{
	public class AlphaRemotingService : IAlphaRemotingService
	{
		private IRemotingSocketService _service;

		private string GetServerLocalIP()
		{
			string hostname = Dns.GetHostName();//得到本机名    
			IPAddress[] addressList = Dns.GetHostAddresses(hostname);
			foreach (IPAddress ip in addressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4
					return ip.ToString();
			}
			return "127.0.0.1";
		}
		public AlphaRemotingService()
		{
			IChannel channel = new TcpClientChannel("Channel1", new BinaryClientFormatterSinkProvider());
			ChannelServices.RegisterChannel(channel, false);
			_service = (IRemotingSocketService)Activator.GetObject(typeof(IRemotingSocketService), "tcp://" + GetServerLocalIP() + ":" + ConfigurationManager.AppSettings["AlphaRemotingServerPort"] + "/VotaiWeb2Srv");
		}

		public bool SendBatchCommand(string id, string cmd, object[] args)
		{
			return _service.SendBatchCommand(id, cmd, args);
		}

		public bool SendCommand(string sn, string cmd, object[] args)
		{
			var cmdIndex = ((int)(DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).TotalMilliseconds).ToString();
			return _service.SendCommand(sn, cmd, cmdIndex, string.Empty, args);
		}

		public bool SendCommand(string SN, string cmd, string description, object[] args)
		{
			var cmdIndex = ((int)(DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).TotalMilliseconds).ToString();
			return _service.SendCommand(SN, cmd, cmdIndex, description, args);
		}
	}
}
