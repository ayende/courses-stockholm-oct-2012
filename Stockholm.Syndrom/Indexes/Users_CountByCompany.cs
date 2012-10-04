using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Stockholm.Syndrom.Models;

namespace Stockholm.Syndrom.Indexes
{
	public class Users_CountByCompany : AbstractIndexCreationTask<User, Users_CountByCompany.ReduceResult>
	{
		public class ReduceResult
		{
			public string Company { get; set; }
			public int Count { get; set; }
			public bool Active { get; set; }
		}

		public Users_CountByCompany()
		{
			Map = users =>
				  from user in users
				  select new
					  {
						  Company = user.Email.Split('@')[1],
						  Active = user.Active ?? true,
						  Count = 1
					  };

			Reduce = bunniesAreGreen =>
					 from result in bunniesAreGreen
					 group result by new { result.Company , result.Active}
						 into g
						 select new
							 {
								 g.Key.Company,
								 g.Key.Active,
								 Count = g.Sum(x => x.Count)
							 };
		}
	}
}