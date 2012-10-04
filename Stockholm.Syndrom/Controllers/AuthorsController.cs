using Stockholm.Syndrom.Indexes; 
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;
using Raven.Client.Linq;
using System.Linq;
using Raven.Client;

namespace Stockholm.Syndrom.Controllers
{
	public class AuthorsController : RavenController
	{
		public object Stats()
		{
			var results = Session
					.Query<Books_Authors.ReduceResult, Books_Authors>()
						.Include(x=>x.AuthorId)
				.ToList();
			var readableResults = from result in results
			                      select new
				                      {
					                      Author = Session.Load<Author>(result.AuthorId).Name,
					                      Books = result.Count
				                      };
			return Json(new
				{
					Results = readableResults.ToArray(),
					RequestCount = Session.Advanced.NumberOfRequests
				});
		}

		public object Generate()
		{
			var weber = new Author
				{
					Name = "David Weber"
				};
			Session.Store(weber);

			Session.Store(new Book
				{
					Name = "Midst toil and tribulation",
					Authors = { weber.Id }
				});

			Session.Store(new Book
				{
					Name = "Field of Dishonor",
					Authors = { weber.Id }
				});

			var rolling = new Author
				{
					Name = "J K Rolling",
				};
			Session.Store(rolling);

			Session.Store(new Book
				{
					Name = "The Philosopher Stone",
					Authors = { rolling.Id }
				});
			Session.Store(new Book
				{
					Name = "Goblet of Fire",
					Authors = { rolling.Id }
				});

			return Json("done");
		}

		public object Search(string q)
		{
			var b = Session.Query<Book, Books_Search>()
				.Search(x => x.Name, q)
				.ToList();

			return Json(b);
		}
	}
}