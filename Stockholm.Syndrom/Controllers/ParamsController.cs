using System.Collections.Generic;
using Raven.Abstractions.Data;
using Stockholm.Syndrom.Infrastructure;
using Stockholm.Syndrom.Models;
using System.Linq;

namespace Stockholm.Syndrom.Controllers
{
	public class ParamsController : RavenController
	{
		public void Update()
		{
			DocumentStore.DatabaseCommands.UpdateByIndex(
				"Raven/DocumentsByEntityName",
				new IndexQuery{ Query = "Tag:Params"}, 
				new ScriptedPatchRequest
					{
						Script = @"
						
						if(this.InstructionName == null || this.InstructionName == '')
						{
							this.InstructionName = 'Something';
						}
						
						"
					}
			);
		}

		public object Load(int id)
		{
			dynamic param = Session.Load<Param>(id);

			return Json(new
				{
					param.InstructionName,
					param.Name,
					param.First,
					param.Age,
					param.Strange
				});
		}

		public object Query(string q, string[] fields)
		{
			var lq = Session.Advanced.LuceneQuery<Param>()
				.Where(q)
				.SelectFields<Param>(fields)
				.ToList();


			return Json(lq.Select((dynamic x)=>
				{
					var dictionary = new Dictionary<string, object>();
					foreach (var field in fields)
					{
						dictionary[field] = x[field];
					}
					return dictionary;
				}));
		}

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