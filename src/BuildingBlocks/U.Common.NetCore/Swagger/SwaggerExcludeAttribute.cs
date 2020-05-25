using System;

namespace U.Common.NetCore.Swagger
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
}