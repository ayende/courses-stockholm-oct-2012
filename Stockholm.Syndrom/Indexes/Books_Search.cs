using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Indexes
{
	public class Books_Search : AbstractIndexCreationTask<Book>
	{
		public Books_Search()
		{
			Map = books =>
			      from book in books
			      select new
				      {
					      book.Name
				      };
			Index(x=>x.Name, FieldIndexing.Analyzed);
		}
	}
}