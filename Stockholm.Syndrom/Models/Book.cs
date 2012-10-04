using System.Collections.Generic;

namespace Stockholm.Syndrom.Models
{
	public class Book
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<string> Authors { get; set; }

		public List<Review> Reviews { get; set; }

		public class Review
		{
			public int Rating { get; set; }
			public string Content { get; set; }
		}

		public Book()
		{
			Reviews = new List<Review>();
			Authors = new List<string>();
		}
	}

	public class Author
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}
}