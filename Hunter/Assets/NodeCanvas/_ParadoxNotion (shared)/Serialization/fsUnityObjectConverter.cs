using System;
using System.Collections.Generic;
using ParadoxNotion.Serialization.FullSerializer;


namespace ParadoxNotion.Serialization{

    ///Hanldes UnityObjects serialization
	public class fsUnityObjectConverter : fsConverter {

		public override bool CanProcess(Type type){
			return typeof(UnityEngine.Object).IsAssignableFrom(type);
		}

		public override bool RequestCycleSupport(Type storageType){
			return false;
		}

		public override bool RequestInheritanceSupport(Type storageType){
			return false;
		}

		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType){
			
			var database = Serializer.Context.Get<List<UnityEngine.Object>>();
			var o = instance as UnityEngine.Object;
			if (!database.Contains((UnityEngine.Object)instance))
				database.Add(o);			

			serialized = new fsData(database.IndexOf(o));
			return fsResult.Success;
		}

		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType){
			var database = Serializer.Context.Get<List<UnityEngine.Object>>();
			var index = (int)data.AsInt64;
			
			if (index >= database.Count)
				return fsResult.Warn("A Unity Object reference has not been deserialized");
			
			instance = database[index];
			return fsResult.Success;
		}

		public override object CreateInstance(fsData data, Type storageType){
			return null;
		}
	}
}