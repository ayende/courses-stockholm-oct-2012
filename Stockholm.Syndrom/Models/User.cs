using System;
using System.Collections.Generic;

namespace Stockholm.Syndrom.Models
{
	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Tagged<Nick>> Nicks { get; set; }

		public User()
		{
			Nicks = new List<Tagged<Nick>>();
		}
	}

	public class Nick
	{
		public string Name { get; set; }
		public string For { get; set; }
	}

	public class Tagged<T>
	{
		public string[] Tags { get; set; }
		public T Value { get; set; }
	}
}