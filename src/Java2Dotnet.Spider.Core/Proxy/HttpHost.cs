﻿using System;

namespace Java2Dotnet.Spider.Core.Proxy
{
	public class HttpHost
	{
		public string Host { get; set; }
		public int Port { get; set; }

		public HttpHost(string host, int port)
		{
			Host = host;
			Port = port;
		}
	}
}
