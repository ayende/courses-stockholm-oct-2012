using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Indexes
{
	public class Books_Search : AbstractIndexCreationTask<Book>
	{
		public class AuthorSearchResult
		{
			public string Authors { get; set; }
		}
		public Books_Search()
		{
			Map = books =>
				  from book in books
				  select new
					  {
						  book.Name,
						  book.Authors
					  };
			Index(x => x.Name, FieldIndexing.Analyzed);
		}
	}

	public class Authors_Search : AbstractIndexCreationTask<Author>
	{
		public Authors_Search()
		{
			Map = authors =>
				  from author in authors
				  select new
					  {
						  author.Name,
					  };
			Index(x => x.Name, FieldIndexing.Analyzed);
		}
	}
}