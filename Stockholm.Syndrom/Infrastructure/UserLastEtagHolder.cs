using System;
using System.Collections.Concurrent;
using System.Web;
using Raven.Client.Util;

namespace Stockholm.Syndrom.Infrastructure
{
	public class UserLastEtagHolder : ILastEtagHolder
	{
		readonly ConcurrentDictionary<string, Guid?> scope = new ConcurrentDictionary<string, Guid?>();

		public void UpdateLastWrittenEtag(Guid? etag)
		{
			var s = HttpContext.Current.Request.QueryString["userId"];

			scope[s] = etag;
		}

		public Guid? GetLastWrittenEtag()
		{
			var s = HttpContext.Current.Request.QueryString["userId"];

			Guid? guid;
			scope.TryGetValue(s, out guid);
			return guid;
		}
	}
}