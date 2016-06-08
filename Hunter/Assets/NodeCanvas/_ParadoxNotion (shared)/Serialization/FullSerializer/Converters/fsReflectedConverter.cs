using System;
using System.Collections;


namespace ParadoxNotion.Serialization.FullSerializer.Internal {
    public class fsReflectedConverter : fsConverter {
        public override bool CanProcess(Type type) {
            if (type.Resolve().IsArray ||
                typeof(ICollection).IsAssignableFrom(type)) {

                return false;
            }

            return true;
        }

        public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType) {
            serialized = fsData.CreateDictionary();
            var result = fsResult.Success;

            var metaType = fsMetaType.Get(instance.GetType());
            for (var i = 0; i < metaType.Properties.Length; ++i) {
                var property = metaType.Properties[i];
                if (property.CanRead == false) continue;

                fsData serializedData;

                var itemResult = Serializer.TrySerialize(property.StorageType, property.Read(instance), out serializedData);
                result.AddMessages(itemResult);
                if (itemResult.Failed) {
                    continue;
                }

                serialized.AsDictionary[property.Name] = serializedData;
            }

            return result;
        }

        public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType) {
            var result = fsResult.Success;

            // Verify that we actually have an Object
            if ((result += CheckType(data, fsDataType.Object)).Failed) {
                return result;
            }

            var metaType = fsMetaType.Get(storageType);

            for (var i = 0; i < metaType.Properties.Length; ++i) {
                var property = metaType.Properties[i];
                if (property.CanWrite == false) continue;

                fsData propertyData;
                if (data.AsDictionary.TryGetValue(property.Name, out propertyData)) {
                    object deserializedValue = null;

                    var itemResult = Serializer.TryDeserialize(propertyData, property.StorageType, ref deserializedValue);
                    result.AddMessages(itemResult);
                    if (itemResult.Failed) continue;

                    property.Write(instance, deserializedValue);
                }
            }

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            var metaType = fsMetaType.Get(storageType);
            return metaType.CreateInstance();
        }
    }
}