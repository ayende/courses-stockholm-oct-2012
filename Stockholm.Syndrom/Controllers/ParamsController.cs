﻿using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;
using System.Linq;

namespace Stockholm.Syndrom.Controllers
{
	public class ParamsController : RavenController
	{
		 public object SaveBetter()
		 {
			 dynamic entity = new Param
				 {
					 InstructionName = "Use RavenDB"
				 };
			 entity.Age = 15;
			 entity.Name = "John";
			 entity.First = false;
			 entity.Strange = new[,] {{1, 23}, {43, 31}, {123, -21}};
			 Session.Store(entity);
			 return null;
		 }

		 public object Search(string q)
		 {
			 var x = Session.Advanced.LuceneQuery<Param>()
				 .Where(q)
				 .ToList();

			 return Json(x);
		 }
	}
}