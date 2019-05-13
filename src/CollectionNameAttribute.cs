using System;

namespace MongoDbGenericRepository.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CollectionNameAttribute: Attribute
    {
        public string _collectionName{get; private set;}
        public CollectionNameAttribute(string collectionName)
        {
            _collectionName = collectionName;
        }
    }
    
}