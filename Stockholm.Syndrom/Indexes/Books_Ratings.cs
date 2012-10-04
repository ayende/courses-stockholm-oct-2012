using System.Linq;
using Raven.Client.Indexes;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Indexes
{
	public class Books_Ratings : AbstractIndexCreationTask<Book>
	{
		public class SearchResult
		{
			public string Name { get; set; }
			public string Authors { get; set; }
			public int Ratings { get; set; }
			public double AvgRatings { get; set; }
		}

		public Books_Ratings()
		{
			Map = books =>
			      from book in books
			      select new
				      {
					      book.Name,
					      book.Authors,
					      Ratings = book.Reviews.Sum(x => x.Rating),
					      AvgRatings = book.Reviews.Count == 0 ? 0 : book.Reviews.Average(x => x.Rating),
				      };
		}
	}
}