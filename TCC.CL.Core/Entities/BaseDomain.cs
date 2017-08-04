using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Interfaces;

namespace TCC.CL.Core.Entities
{
    [Serializable]
    public abstract class BaseDomain<T, TKey> : IEntity<TKey>
        where T : class
        where TKey : struct
    {
        public virtual TKey Id
        {
            get;
            set;
        }
        
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var outro = obj as BaseDomain<T, TKey>;
            if (outro == null)
                return false;
            else
                return outro.Id.Equals(Id);
        }

        public static string LimparCaracteresEspeciais(string value)
        {

            return value.Replace("-", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace("%", "");
        }

        public static T DeepClone(T obj)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(obj, null))
            {
                return default(T);
            }

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);

                return (T)formatter.Deserialize(ms);
            }
        }
        
    }
}
