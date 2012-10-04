using System.Media;
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Controllers
{
	public class UsersController : RavenController
	{
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
			Session.Load<User>(id).ReadBookIds.Add(bookId);
			return Json("Recorded that you read this book");
		}
	}
}