using System.Collections.Generic;

namespace Stockholm.Syndrom.Models
{
	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<string> Nicks { get; set; }

		public User()
		{
			Nicks = new List<string>();
		}
	}
}