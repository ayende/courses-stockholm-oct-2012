using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Stockholm.Syndrom.Infrastructure
{
	public abstract class RavenController : Controller
	{
		protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
		}

		protected ActionResult Xml(object val)
		{
			return new XmlResult(val);
		}

		private static readonly Lazy<IDocumentStore> documentStore =
			new Lazy<IDocumentStore>(() =>
				{
					var store = new DocumentStore
						{
							Url = "http://localhost:8080",
							DefaultDatabase = "Stockholm",
							Conventions =
								{
									MaxNumberOfRequestsPerSession =2,
									FindTypeTagName = type =>
										{
											if(type == typeof(Dictionary<string,object>))
												return "Params";
											return DocumentConvention.DefaultTypeTagName(type);
										}
								}
						}
						.RegisterListener(new AuditListener())
						.Initialize();

					IndexCreation.CreateIndexes(typeof(RavenController).Assembly, store);
					return store;
				});

		public static IDocumentStore DocumentStore
		{
			get
			{
				return documentStore.Value;
			}
		}

		public new IDocumentSession Session { get; set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			Session = DocumentStore.OpenSession();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			using (Session)
			{
				if (Session == null)
					return;
				if (filterContext.Exception != null)
					return;
				Session.SaveChanges();
			}
		}
	}
}