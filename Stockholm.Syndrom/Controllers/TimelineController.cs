using System;
using Stockholm.Syndrom.Indexes;
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;
using System.Linq;

namespace Stockholm.Syndrom.Controllers
{
	public class TimelineController : RavenController
	{
		 public object Write()
		 {
			 Session.Store(new Book
				 {
					 Name = "New"
				 });
			 return RedirectToAction("List",new{userId = Request.QueryString["userId"]});
		 }

		 public object List()
		 {
			 var r = Session.Query<Book, Books_Search>()
				 .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite(TimeSpan.FromMinutes(1)))
				 .ToList();
			 return Json(r);
		 }
	}
}