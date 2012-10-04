using System.Media;
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Controllers
{
	public class UsersController : RavenController
	{
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