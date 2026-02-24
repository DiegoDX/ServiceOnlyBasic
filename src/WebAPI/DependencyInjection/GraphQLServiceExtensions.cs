using WebAPI.GraphQL.Mutations;
using WebAPI.GraphQL.Queries;
//using HotChocolate.AspNetCore.Authorization;
//using HotChocolate.Authorization;

namespace WebAPI.DependencyInjection
{
    public static class GraphQLServiceExtensions
    {
        public static IServiceCollection AddGraphQLConfiguration(this IServiceCollection services)
        {
            services
                .AddGraphQLServer()
                    //.AddQueryType(d => d.Name("Query"))
                    .AddQueryType<CityQuery>()
                    //.AddTypeExtension<CountryQuery>()
                    //.AddMutationType(d => d.Name("Mutation"))
                    .AddMutationType<CityMutation>();
                    //.AddAuthorization();
            return services;
        }
    }
}
