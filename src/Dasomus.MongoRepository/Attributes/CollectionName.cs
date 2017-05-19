using System;
using System.Collections.Generic;
using System.Text;

namespace Dasomus.MongoRepository
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CollectionName : Attribute
    {
        public CollectionName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new Exception("Name cannot be null or empty string");

            this.Name = name;
        }
        
        public string Name { get; private set; }
    }
}
