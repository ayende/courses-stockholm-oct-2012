using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Indexes
{
	public class All_Search : AbstractMultiMapIndexCreationTask<All_Search.SearchResult>
	{
		public class SearchResult
		{
			public string Name { get; set; }
		}

		public All_Search()
		{
			AddMap<User>(users =>
						 from user in users
						 select new { user.Name }
				);
			AddMap<Book>(books =>
									 from book in books
									 select new { book.Name }
							);
			AddMap<Author>(authors =>
									 from author in authors
									 select new { author.Name }
							);

			Index(x=>x.Name, FieldIndexing.Analyzed);
		}
	}
}