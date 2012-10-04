using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Stockholm.Syndrom.Controllers
{
	public class Param : DynamicObject
	{
		private Dictionary<string, object>  vals = new Dictionary<string, object>();

		public string InstructionName { get; set; }

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return vals.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if(binder.Name == "Id")
				return false;
			vals[binder.Name] = value;
			return true;
		}

		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			var key = (string) indexes[0];
			if(key == "Id")
				return false;
			vals[key] = value;
			return true;
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			return vals.TryGetValue((string) indexes[0], out result);
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return GetType().GetProperties().Select(x => x.Name).Concat(vals.Keys);
		}
	}
}