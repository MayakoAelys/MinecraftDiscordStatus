using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace MinecraftDiscordStatus.Shared.Logging
{
    public class IncludePublicFieldsPolicy : IDestructuringPolicy
    {
        //ref.: https://stackoverflow.com/a/60242736
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            TypeInfo typeInfo = value.GetType().GetTypeInfo();

            IEnumerable<LogEventProperty> publicFields =
                typeInfo
                    .DeclaredFields
                    .Where(field => field.IsPublic)
                    .Select(field =>
                    {
                        object fieldValue = field.GetValue(value) ?? null;
                        LogEventPropertyValue logEventPropertyValue = propertyValueFactory.CreatePropertyValue(fieldValue);

                        var logEventProperty = new LogEventProperty(field.Name, logEventPropertyValue);

                        return logEventProperty;
                    });

            IEnumerable<LogEventProperty> publicProperties =
                typeInfo
                    .DeclaredProperties
                    .Where(field => field.CanRead)
                    .Select(field =>
                    {
                        object fieldValue = field.GetValue(value) ?? null;
                        LogEventPropertyValue logEventPropertyValue = propertyValueFactory.CreatePropertyValue(fieldValue);

                        var logEventProperty = new LogEventProperty(field.Name, logEventPropertyValue);

                        return logEventProperty;
                    });

            result = new StructureValue(publicFields.Union(publicProperties));

            return true;
        }
    }
}
