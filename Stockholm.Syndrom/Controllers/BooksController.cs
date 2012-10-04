using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Controllers
{
	public class BooksController : RavenController
	{
		 public object Register(string name, string[] authors)
		 {
			 var book = new Book
				 {
					 Name = name, 
					 Authors = authors
				 };
			 Session.Store(book);

			 return Json(book.Id);
		 }
	}
}