using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Controllers
{
	public class UsersController : RavenController
	{
		 public object List(string user)
		 {
			 var load = Session.Load<User>(user);
			 return Xml(load);
		 }
	}
}