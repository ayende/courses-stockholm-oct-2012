using System;
using System.Collections.Generic;

namespace Stockholm.Syndrom.Models
{
	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		
		public List<string> WatchedMovieIds { get; set; }
		public List<string> ReadBookIds { get; set; }

		public bool? Active { get; set; }

		public User()
		{

			WatchedMovieIds = new List<string>();
			ReadBookIds = new List<string>();
		}
	}

	
}