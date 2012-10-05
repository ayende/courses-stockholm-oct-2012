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
		public object Easy(string q)
		{
			var results = Session.Query<Book, Books_Search>()
				.Search(x => x.Name, q)
				.Lazily();

			var authors = Session.Query<Author, Authors_Search>()
				.Search(x => x.Name, q)
				.Lazily();


			return Json(results.Value.Cast<object>().Concat(authors.Value));
		}

		public object Easier(string q)
		{
			var query = Session.Query<All_Search.SearchResult, All_Search>()
				.Search(x => x.Name, q);

			var results = query
				.As<object>()
				.ToList();

			if (results.Count == 0)
			{
				var suggest = query.Suggest();

				if (suggest.Suggestions.Length == 0)
				{
					return Json("Not a clue what to do");
				}
				if (suggest.Suggestions.Length <= 3)
				{
					return Easier(string.Join(" ", suggest.Suggestions));
				}
				return Json(new
				{
					DidYouMean = suggest.Suggestions
				});
			}

		
			return Json(results);
		}

		public object Book(int id)
		{
			var book = Session.Advanced.Lazily.Load<Book>(id);
			var bookId = "books/" + id;
			RavenQueryStatistics stats;

			Session.Query<User>()
				.Statistics(out stats)
				.Where(x => x.ReadBookIds.Any(b => b == bookId))
				.Take(0)
				.Lazily();

			Session.Advanced.Eagerly.ExecuteAllPendingLazyOperations();

			return Json(new
				{
					book.Value.Name,
					UsersCount = stats.TotalResults,
					Session.Advanced.NumberOfRequests
				});
		}

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