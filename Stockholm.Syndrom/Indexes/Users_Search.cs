using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Indexes
{
	public class Users_Search : AbstractIndexCreationTask<User, Users_Search.SearchResult>
	{
		public class SearchResult
		{
			public string Query { get; set; }
		}
		public Users_Search()
		{
			Map = users =>
			      from user in users
			      select new
				      {
					      Query = new object[]
						      {
									user.Name, 
									user.Email, 
									user.Email.Split('@')
						      }
				      };

			Index(x => x.Query, FieldIndexing.Analyzed);
		}
	}
}