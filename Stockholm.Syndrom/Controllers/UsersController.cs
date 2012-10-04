using System.Linq;
using System.Media;
using System.Text;
using Stockholm.Syndrom.Indexes;
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;
using Raven.Client;

namespace Stockholm.Syndrom.Controllers
{
	public class UsersController : RavenController
	{
		public object Query(string terms)
		{
			var q = Session.Query<Users_Search.SearchResult, Users_Search>()
				.Search(x => x.Query, terms);

			var results = q
				.As<User>()
				.ToList();

			if (results.Count == 0)
			{
				var suggest = q.Suggest();

				if (suggest.Suggestions.Length == 0)
				{
					return Json("Not a clue what to do");
				}
				if (suggest.Suggestions.Length <= 3)
				{
					return Query(string.Join(" ", suggest.Suggestions));
				}
				return Json(new
					{
						DidYouMean = suggest.Suggestions
					});
			}

			return Json(results);
		}

		public void Update(int id)
		{
			Session.Advanced.UseOptimisticConcurrency = true;

			var load = Session.Load<User>(id);
			load.WatchedMovieIds.Add("ONE");
		}

		public object Import(int size)
		{
			for (int i = 0; i < size; i++)
			{
				Session.Store(new User
					{
						Name = "User " + i
					});
			}
			return "Imported";
		}

		public object Add(string name)
		{
			var user = new User
				{
					Name = name,
				};
			Session.Store(user);
			return Json(user.Id);
		}

		public object Read(int id, string bookId)
		{
			var load = Session.Load<User>(id);
			load.ReadBookIds.Add(bookId);
			return Json("Recorded that you read this book");
		}
	}
}