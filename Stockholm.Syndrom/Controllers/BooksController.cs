using System.Linq;
using Raven.Client;
using Stockholm.Syndrom.Indexes;
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Controllers
{
	public class BooksController : RavenController
	{
		public object ByReviews()
		{
			var q = Session.Query<Books_Ratings.SearchResult, Books_Ratings>()
				.OrderByDescending(x => x.Ratings)
				.As<Book>();

			return Json(q);
		}

		public object BooksByAuthorCount(int count)
		{
			var books = from book in Session.Query<Book>()
						where book.Authors.Count > count
						select book;

			return Json(books);
		}

		public object Register(string name, string[] authors)
		{
			var book = new Book
				{
					Name = name,
					Authors = authors.ToList()
				};
			Session.Store(book);

			return Json(book.Id);
		}
	}
}