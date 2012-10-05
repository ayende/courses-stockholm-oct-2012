using Raven.Abstractions.Data;
using Stockholm.Syndrom.Infrastructure;
using Raven.Client;
using Raven.Client.Linq;

namespace Stockholm.Syndrom.Controllers
{
	public class FacetsController : RavenController
	{
		public void CreateFacetsDoc()
		{
			Session.Store(new FacetSetup
				{
					Id = "Facets/Lasers",
					Facets =
						 {
							 new Facet
								 {
									 Name = "Manufacterer"
								 },
							new Facet
								{
									Name = "Gain",
									Mode = FacetMode.Ranges,
									Ranges =
										{
											"[Ix0 TO Ix50]",
											"[Ix51 TO Ix100]",
											"[Ix101 TO NULL]"
										}
								},
							new Facet
								{
									Name = "Voltage",
								}
						 }
				});
		}

		public object Run(string q)
		{
			var facets = Session.Query<Laser>("Lasers")
				.Where(x => x.Current == "AC")
				.ToFacets("Facets/Lasers");

			return Json(facets);
		}
	}

	public class Laser
	{
		public string Manufacterer { get; set; }
		public int Gain { get; set; }
		public string Current { get; set; }
		public int Voltage { get; set; }
	}
}