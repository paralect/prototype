using System;
using MongoDB.Bson.Serialization.Conventions;

namespace Prototype.Platform.Mongo
{
    public class NoDefaultPropertyIdConvention : IIdMemberConvention
    {
        public string FindIdMember(Type type)
        {
            return null;
        }
    }
}