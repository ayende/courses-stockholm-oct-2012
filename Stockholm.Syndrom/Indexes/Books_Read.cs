using System.Linq;
using Raven.Client.Indexes;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Indexes
{
	public class Books_Read : AbstractMultiMapIndexCreationTask<Books_Read.ReduceResult>
	{
		public class ReduceResult
		{
			public string BookId { get; set; }
			public string Authors { get; set; }
			public string BookName { get; set; }
			public string[] ReadBy { get; set; }
		}


		public Books_Read()
		{
			AddMap<Book>(books =>
						 from book in books
						 select new
						 {
							 BookId = book.Id,
							 book.Authors,
							 BookName = book.Name,
							 ReadBy = new string[0]
						 }
				);

			AddMap<User>(users =>
						 from user in users
						 from bookId in user.ReadBookIds
						 select new
							 {
								 BookId = bookId,
								 Authors = new string[0],
								 BookName = (string)null,
								 ReadBy = new[] { user.Id }
							 }
				);

			Reduce = results =>
					 from result in results
					 group result by result.BookId
						 into g
						 select new
							 {
								 BookId = g.Key,
								 Authors = g.SelectMany(x=>x.Authors),
								 g.FirstOrDefault(x=>x.BookName!=null).BookName,
								 ReadBy = g.SelectMany(x=>x.ReadBy)
							 };
		}
	}
}