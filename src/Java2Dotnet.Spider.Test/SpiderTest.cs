﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Java2Dotnet.Spider.Common;
using Java2Dotnet.Spider.Core;
using Java2Dotnet.Spider.Core.Downloader;
using Java2Dotnet.Spider.Core.Pipeline;
using Java2Dotnet.Spider.Core.Processor;
using Java2Dotnet.Spider.Core.Scheduler;
#if !NET_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Java2Dotnet.Spider.Test
{
	[TestClass]
	public class SpiderTest
	{
		[TestMethod]
		public void TestStartAndStop()
		{
			HttpClientDownloader downloader = new HttpClientDownloader();

			Core.Spider spider = Core.Spider.Create(new SimplePageProcessor("http://www.oschina.net/", "http://www.oschina.net/*")).AddPipeline(new TestPipeline()).SetThreadNum(1);
			spider.Site = new Site() { Encoding = Encoding.UTF8 };
			Page p = downloader.Download(new Request("http://www.baidu.com/", 2, new Dictionary<string, dynamic>()), spider);
			Console.WriteLine(p.Content);
			spider.Start();
			Thread.Sleep(10000);
			spider.Stop();
			Thread.Sleep(10000);
			spider.Start();
			Thread.Sleep(10000);
		}

		private class TestPipeline : IPipeline
		{
			public void Process(ResultItems resultItems, ISpider spider)
			{
				foreach (var entry in resultItems.Results)
				{
					Console.WriteLine($"{entry.Key}:{entry.Value}");
				}
			}

			public void Dispose()
			{
			}
		}

		//[Ignore]
		//[TestMethod]
		//public void TestWaitAndNotify()
		//{
		//	for (int i = 0; i < 10000; i++)
		//	{
		//		Console.WriteLine("round " + i);
		//		TestRound();
		//	}
		//}

		private void TestRound()
		{
			Core.Spider spider = Core.Spider.Create(new TestPageProcessor(), new TestScheduler()).SetThreadNum(10);
			spider.Run();
		}

		private class TestScheduler : IScheduler
		{
			private AtomicInteger _count = new AtomicInteger();
			private Random _random = new Random();

			public void Init(ISpider spider)
			{
			}

			public void Push(Request request, ISpider spider)
			{
			}


			public Request Poll(ISpider spider)
			{
				lock (this)
				{
					if (_count.Inc() > 1000)
					{
						return null;
					}
					if (_random.Next(100) > 90)
					{
						return null;
					}
					return new Request("test", 1, null);
				}
			}

			public void Dispose()
			{
			}
		}

		private class TestPageProcessor : IPageProcessor
		{
			public void Process(Page page)
			{
				page.IsSkip = true;
			}

			public Site Site => new Site { SleepTime = 0 };
		}

		public class TestDownloader : IDownloader
		{
			public DownloadValidation DownloadValidation { get; set; }

			public Page Download(Request request, ISpider spider)
			{
				var page = new Page(request, ContentType.Html);
				page.Content = "";
				return page;
			}

			public int ThreadNum { get; set; }
		}
	}
}
