using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebApi.SwaggerFilters
{
    public class ItemTradingAuctionSlotInfoRequestFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiBodyParam = context.ApiDescription.ParameterDescriptions.FirstOrDefault(/*description =>
            description.Source.CanAcceptDataFrom(BindingSource.Body)*/);
            if (apiBodyParam == null)
            {
                return;
            }

            var swaggerQueryParam = operation.Parameters.FirstOrDefault(/*parameter =>
                parameter.Name == apiBodyParam.Name && parameter.In == ParameterLocation.Query*/);
            if (swaggerQueryParam == null)
            {
                return;
            }

            var attr = context.MethodInfo.GetCustomAttribute(typeof(ConsumesAttribute)) as ConsumesAttribute;
            if (attr is null) return;

            operation.Parameters.Remove(swaggerQueryParam);
            var bodyContent = new Dictionary<string, OpenApiMediaType>();
            if (attr != null)
            {
                foreach (var contentType in attr.ContentTypes)
                {
                    var mediaType = new OpenApiMediaType
                    {
                        Schema = swaggerQueryParam.Schema,
                        Example = swaggerQueryParam.Example,
                        Examples = swaggerQueryParam.Examples,
                        Encoding = new Dictionary<string, OpenApiEncoding>(),
                        Extensions = swaggerQueryParam.Extensions
                    };
                    if (contentType is "multipart/form-data" or "application/x-www-form-urlencoded")
                    {
                        var props = apiBodyParam.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var prop in props)
                        {
                            mediaType.Encoding.Add(prop.Name, new OpenApiEncoding
                            {
                                Style = ParameterStyle.Form
                            });
                        }
                    }

                    bodyContent.Add(contentType, mediaType);
                }
            }
            else
            {
                bodyContent.Add("application/json", new OpenApiMediaType
                {
                    Schema = swaggerQueryParam.Schema,
                    Example = swaggerQueryParam.Example,
                    Examples = swaggerQueryParam.Examples,
                    // Encoding = swaggerQueryParam.Content.First().Value.Encoding,
                    Extensions = swaggerQueryParam.Extensions
                });
            }

            operation.RequestBody = new OpenApiRequestBody
            {
                UnresolvedReference = swaggerQueryParam.UnresolvedReference,
                Reference = swaggerQueryParam.Reference,
                Description = swaggerQueryParam.Description,
                Required = swaggerQueryParam.Required,
                Content = bodyContent,
                Extensions = swaggerQueryParam.Extensions
            };
        }
    }
}
