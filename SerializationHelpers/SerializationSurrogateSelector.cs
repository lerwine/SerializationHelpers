using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SerializationHelpers
{
    public class SerializationSurrogateSelector : IDataContractSurrogate
    {
        #region IDataContractSurrogate Members

        public object GetCustomDataToExport(Type clrType, Type dataContractType) { return null; }

        public object GetCustomDataToExport(System.Reflection.MemberInfo memberInfo, Type dataContractType) { return null; }

        public Type GetDataContractType(Type type)
        {
            if (type == null)
                return null;

            if ((typeof(Exception)).IsAssignableFrom(type))
            {
                if ((typeof(System.Runtime.InteropServices.ExternalException)).IsAssignableFrom(type))
                {
                    if (type.Equals(typeof(System.Data.SqlClient.SqlException)))
                        return typeof(Surrogates.Exceptions.SqlExceptionSurrogate);

                    return (typeof(Surrogates.Exceptions.ExternalExceptionSurrogate<>)).MakeGenericType(type);
                }

                if ((typeof(ArgumentException)).IsAssignableFrom(type))
                {
                    if ((typeof(ArgumentNullException)).IsAssignableFrom(type))
                        return (typeof(Surrogates.Exceptions.ArgumentNullExceptionSurrogate<>)).MakeGenericType(type);
                    if ((typeof(ArgumentOutOfRangeException)).IsAssignableFrom(type))
                        return (typeof(Surrogates.Exceptions.ArgumentOutOfRangeExceptionSurrogate<>)).MakeGenericType(type);
                    return (typeof(Surrogates.Exceptions.ArgumentExceptionSurrogate<>)).MakeGenericType(type);
                }
                return (typeof(Surrogates.Exceptions.ExceptionSurrogate<>)).MakeGenericType(type);
            }

            if (type.Equals(typeof(System.Data.SqlClient.SqlError)))
                return typeof(Surrogates.SqlErrorSurrogate);

            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            if (obj != null && obj is Surrogates.IObjectSurrogate)
                return (obj as Surrogates.IObjectSurrogate).Deserialized_Object;

            return obj;
        }

        public void GetKnownCustomDataTypes(System.Collections.ObjectModel.Collection<Type> customDataTypes) { }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            if (obj == null)
                return null;

            if (obj is Exception || obj is System.Data.SqlClient.SqlError)
                return Activator.CreateInstance(this.GetDataContractType(obj.GetType()), obj);

            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData) { return null; }

        public System.CodeDom.CodeTypeDeclaration ProcessImportedType(System.CodeDom.CodeTypeDeclaration typeDeclaration, System.CodeDom.CodeCompileUnit compileUnit) { return typeDeclaration; }

        #endregion
    }
}
