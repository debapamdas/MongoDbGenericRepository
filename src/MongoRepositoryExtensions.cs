using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using MongoDbGenericRepository.Abstractions;

namespace MongoDbGenericRepository
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongoRepository<TRepository>(this IServiceCollection services, Action<MongoDbOptions> options)
            where TRepository: class, IRepository
        {
            var opt = new MongoDbOptions();
            options(opt);
            services.AddMongoRepository<TRepository>(opt);
            return services;
        }
        public static IServiceCollection AddMongoRepository<TRepository>(this IServiceCollection services, MongoDbOptions options)
            where TRepository: class, IRepository
        {
            var client = new MongoClient(options.connectionString);
            services.AddSingleton<IMongoDatabase>(sp => client.GetDatabase(options.databseName));
            services.AddScoped<TRepository>();
            return services;
        }
    }

    public class MongoDbOptions
    {
        public string connectionString {get; set;}
        public string databseName {get; set;}
    }
}
