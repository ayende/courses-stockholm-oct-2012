using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			using(var store = new DocumentStore
				{
					Url = "http://localhost:8080",
					DefaultDatabase = "Wow",
					Conventions =
						{
							FailoverBehavior = FailoverBehavior.ReadFromAllServers
						}
				}.Initialize())
			{
				store.Changes().ForAllDocuments()
					.Subscribe(notification =>
						{
							Console.WriteLine(notification.Name + " " + notification.Type + " " + notification.Etag);
						});


				while (true)
				{
					var spo = Stopwatch.StartNew();
					using(var s = store.OpenSession())
					{
						var load = s.Load<RavenJObject>("books/1");
						Console.WriteLine(load.Value<string>("Name"));
					}
					Console.WriteLine(spo.ElapsedMilliseconds);
					Console.ReadKey();
				}

			}
		}
	}
}
