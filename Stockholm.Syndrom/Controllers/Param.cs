using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Raven.Client.Listeners;
using Raven.Json.Linq;

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

		public object this[string key]
		{
			get
			{
				object value;
				vals.TryGetValue(key, out value);
				return value;
			}
			set { vals[key] = value; }
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

	public class ValidationListener : IDocumentStoreListener
	{
		readonly Dictionary<Type, List<Action<object>>> validations = new Dictionary<Type, List<Action<object>>>();

		public void Register<T>(Action<T> validate)
		{
			List<Action<object>> list;
			if(validations.TryGetValue(typeof(T),out list) == false)
				validations[typeof (T)] = list = new List<Action<object>>();

			list.Add(o => validate((T) o));
		}

		public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
		{
			List<Action<object>> list;
			if (validations.TryGetValue(entityInstance.GetType(), out list) != false)
			{
				foreach (var validation in list)
				{
					validation(entityInstance);
				}
			}
			return false;
		}

		public void AfterStore(string key, object entityInstance, RavenJObject metadata)
		{
		}
	}
}

