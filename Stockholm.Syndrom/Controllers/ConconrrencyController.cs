using System;
using Raven.Abstractions.Exceptions;
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Controllers
{
	public class ConconrrencyController : RavenController
	{
		 public object Load(int id)
		 {
			 var user = Session.Load<User>(id);
			 return Json(new
				 {
					 User = user,
					 Etag = Session.Advanced.GetEtagFor(user)
				 });
		 }

		 public void Save(int id, string name, Guid etag)
		 {
			 Session.Advanced.UseOptimisticConcurrency = true;
			 var user = Session.Load<User>(id);
			 if (Session.Advanced.GetEtagFor(user) != etag)
				throw new ConcurrencyException("Changed");

			 user.Name = name;
		 }

		 public void Save2(int id, User user, Guid etag)
		 {
			 Session.Advanced.UseOptimisticConcurrency = true;
			 Session.Store(user, etag, "users/" + id);
		 }
	}
}