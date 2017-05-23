using System;
using System.Collections.Generic;
using System.Text;

namespace Dasomus.MongoRepository.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexColumn : Attribute
    {
        public IndexColumn()
        { }
    }
}
