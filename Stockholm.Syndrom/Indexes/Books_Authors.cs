using System.Linq;
using Raven.Client.Indexes;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Indexes
{
	public class Books_Authors : AbstractIndexCreationTask<Book, Books_Authors.ReduceResult>
	{
		public class ReduceResult
		{
			public string AuthorId { get; set; }
			public int Count { get; set; }
			public string AuthorName { get; set; }
		}

		public Books_Authors()
		{
			Map = books =>
			      from book in books
			      from author in book.Authors
			      select new
				      {
					      AuthorId = author,
					      Count = 1
				      };

			Reduce = results =>
			         from result in results
			         group result by result.AuthorId
			         into g
			         select new
				         {
					         AuthorId = g.Key,
					         Count = g.Sum(x => x.Count)
				         };

			TransformResults = (database, results) =>
			                   from result in results
			                   let author = database.Load<Author>(result.AuthorId)
			                   select new
				                   {
					                   AuthorName = author.Name,
					                   result.AuthorId,
					                   result.Count
				                   };
		}
	}
}