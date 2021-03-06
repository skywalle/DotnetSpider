﻿using DotnetSpider.Core.Selector;
using Xunit;

namespace DotnetSpider.Core.Test
{
	public class HtmlTest
	{
		[Fact]
		public void Select()
		{
			Selectable selectable = new Selectable("aaaaaaab", "", ContentType.Html);
			string value = selectable.Regex("(.*)").GetValue();
			Assert.Equal("aaaaaaab", value);
		}

		[Fact]
		public void DonotDetectDomain()
		{
			Selectable selectable = new Selectable("<div><a href=\"www.aaaa.com\">aaaaaaab</a></div>", "", ContentType.Html);
			var values = selectable.XPath(".//a").GetValues();
			Assert.Equal("aaaaaaab", values[0]);
		}

		[Fact]
		public void DetectDomain1()
		{
			Selectable selectable = new Selectable("<div><a href=\"www.aaaa.com\">aaaaaaab</a></div>", "", ContentType.Html, "www\\.aaaa\\.com");
			var values = selectable.XPath(".//a").GetValues();
			Assert.Equal("aaaaaaab", values[0]);
		}

		[Fact]
		public void DetectDomain2()
		{
			Selectable selectable = new Selectable("<div><a href=\"www.aaaab.com\">aaaaaaab</a></div>", "", ContentType.Html, "www\\.aaaa\\.com");
			var values = selectable.XPath(".//a").GetValues();
			Assert.Empty(values);
		}
	}
}
