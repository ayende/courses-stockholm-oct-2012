using Stockholm.Syndrom.Indexes;
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;
using Raven.Client;
using System.Linq;
using Raven.Client.Linq;

namespace Stockholm.Syndrom.Controllers
{
	public class Books2Controller : RavenController
	{
		public object Search(string q)
		{
			var results = Session.Query<Book, Books_Search>()
				.Search(x => x.Name, q)
				.Lazily();

			var authors = Session.Query<Author, Authors_Search>()
				.Search(x => x.Name, q)
				.Lazily();

			var authorIds = authors.Value.Select(y => y.Id).ToArray();
			var authorBooks = Session.Query<Books_Search.AuthorSearchResult, Books_Search>()
				.Where(x => x.Authors.In(authorIds))
				.As<Book>()
				.ToList();

			return Json(results.Value.Concat(authorBooks).ToList());
		}
	}
}