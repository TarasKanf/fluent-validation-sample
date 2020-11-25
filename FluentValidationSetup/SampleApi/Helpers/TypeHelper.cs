using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SampleApi.Helpers
{
    public class TypeHelper
    {
        public static IEnumerable<Type> GetDerivedTypes(Type type, IEnumerable<Assembly> assemblies = null)
        {
            if (assemblies == null)
                assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    types.AddRange(
                        from implementingType in assembly.GetTypes()
                        where 
                            implementingType.IsClass 
                            && !implementingType.IsNotPublic 
                            && !implementingType.IsAbstract 
                            && type.IsAssignableFrom(implementingType)
                        select implementingType);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    string loaderMessages = String.Join(", ", ex.LoaderExceptions.Select(le => le.Message));
                    Trace.TraceInformation("Unable to search types from assembly \"{0}\" for plugins of type \"{1}\": {2}", assembly.FullName, type.Name, loaderMessages);
                }
            }

            return types;
        }
        
        public static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type openGenericType, IEnumerable<Assembly> assemblies = null)
        {
            if (assemblies == null)
                assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var implementingTypes = new List<Type>();
            foreach (var assembly in assemblies)
                implementingTypes.AddRange(
                    from x in assembly.GetTypes()
                    from z in x.GetInterfaces()
                    let y = x.BaseType
                    where
                        !x.IsAbstract
                        &&
                        (
                            (
                                y != null
                                && y.IsGenericType 
                                && openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())
                            )
                            ||
                            (
                                z.IsGenericType
                                && openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition())
                            )
                        )
                    select x
                );

            return implementingTypes;
        }
    }
}