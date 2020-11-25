using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SampleApi.Helpers;

namespace SampleApi.Extension
{
    public static class ContainerExtension
    {
        public static IServiceCollection AddSingletons(this IServiceCollection services, Type type, params Assembly[] assemblies)
        {
            var implementingTypes = new List<Type>();
            implementingTypes.AddRange(type.IsGenericTypeDefinition
                ? TypeHelper.GetAllTypesImplementingOpenGenericType(type, assemblies)
                : TypeHelper.GetDerivedTypes(type, assemblies));

            foreach (var implementingType in implementingTypes)
            {
                Type registrationType = type;
                if (type.IsGenericTypeDefinition)
                {
                    if (type.IsInterface)
                    {
                        registrationType = type.MakeGenericType(implementingType.GetInterface(type.Name).GenericTypeArguments);
                    }
                    else
                    {
                        registrationType = type.MakeGenericType(implementingType.BaseType.GenericTypeArguments);
                    }
                }

                services.AddSingleton(registrationType, implementingType);
                services.AddSingleton(implementingType, implementingType);
            }

            return services;
        }
    }
}