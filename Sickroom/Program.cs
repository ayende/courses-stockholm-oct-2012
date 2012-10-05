using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Connection.Async;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Shard;
using Raven.Client.Linq;

namespace Sickroom
{
	class Program
	{
		static void Main(string[] args)
		{

			var locationsToShardId = new Dictionary<string, string>
				{
					{"Stockholm", "Edin"},
					{"London", "Edin"},
					{"Malmo", "Arne"},
					{"Zurich", "Tomas"},
					{"Oslo", "Tomas"},
					{"Copenhagen", "Jonas"},
				};

			var shards = new Dictionary<string, IDocumentStore>
				{
					{"Edin", new DocumentStore {Url = "http://10.111.203.7:8080", DefaultDatabase = "Health"}},
					{"Arne", new DocumentStore {Url = "http://10.111.203.9:8080", DefaultDatabase = "Health"}},
					{"Jonas", new DocumentStore {Url = "http://10.111.203.6:8080", DefaultDatabase = "Health"}},
					{"Tomas", new DocumentStore {Url = "http://10.111.203.4:8080", DefaultDatabase = "Health"}},
				};

			var shardStrategy = new ShardStrategy(shards)
				.ShardingOn<Patient>(x=>x.Location, location => locationsToShardId[location])
				.ShardingOn<Interaction>(x => x.PatientId);

			using (var store = new ShardedDocumentStore(shardStrategy))
			{
				store.Initialize();

				new Interactions_ByDoctor().Execute(store);

				using (var session = store.OpenSession())
				{
					var x= session.Query<Interactions_ByDoctor.ReduceResult, Interactions_ByDoctor>()
						.ToList();

					foreach (var reduceResult in x)
					{
						Console.WriteLine(reduceResult.DoctorId + " " + reduceResult.Count);
					}
				}

				//using (var session = store.OpenSession())
				//{
				//	for (int i = 0; i < 10; i++)
				//	{
				//		var patient = new Patient
				//			{
				//				Name = "Patinet 0" + i
				//			};
				//		session.Store(patient);
				//		for (int j = 0; j < 5; j++)
				//		{
				//			session.Store(new Interaction
				//				{
				//					PatientId = patient.Id,
				//					WillDieSoon = j * j % 32 == 0,
				//					DoctorId = i %2 == 0 ? "doctors/who" : "doctors/no"
				//				});
				//		}
				//	}

				//	session.SaveChanges();
				//}

				//using (var session = store.OpenSession())
				//{
				//	var z = session.Load<Patient>("Edin/patients/6");
				//	Console.WriteLine(z.Name);
				//}
			}

		}
	}

	public class Interactions_ByDoctor : AbstractIndexCreationTask<Interaction, Interactions_ByDoctor.ReduceResult>
	{
		public class ReduceResult
		{
			public string DoctorId { get; set; }
			public int Count { get; set; }
		}

		public Interactions_ByDoctor()
		{
			Map = interactions =>
			      from interaction in interactions
			      select new
				      {
					      Count = 1,
					      interaction.DoctorId
				      };

			Reduce = results =>
			         from result in results
			         group result by result.DoctorId
			         into g
			         select new
				         {
					         DoctorId = g.Key,
					         Count = g.Sum(x => x.Count)
				         };
		}
	}


	public class Patient
	{
		public string Name { get; set; }
		public string Id { get; set; }

		public string Location { get; set; }
	}

	public class Interaction
	{
		public string PatientId { get; set; }
		public bool WillDieSoon { get; set; }
		public string DoctorId { get; set; }
	}


	public class MyShardingResolutionStrategy : DefaultShardResolutionStrategy
	{
		public MyShardingResolutionStrategy(IEnumerable<string> shardIds, ShardStrategy shardStrategy) : base(shardIds, shardStrategy)
		{
		}

		public override IList<string> PotentialShardsFor(ShardRequestData requestData)
		{
			if (requestData.Keys.Contains("Edin/patients/6"))
				return new List<string>{"Ayende"};

			return base.PotentialShardsFor(requestData);
		}
	}
}


