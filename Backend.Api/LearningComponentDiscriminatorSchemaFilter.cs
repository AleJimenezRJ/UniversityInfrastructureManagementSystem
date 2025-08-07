using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.SchemaFilter;

public class LearningComponentDiscriminatorSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(LearningComponentDto))
        {
            schema.Discriminator = new OpenApiDiscriminator
            {
                PropertyName = "@odata.type",
                Mapping =
                {
                    { "#namespace.ProjectorDto", "#/components/schemas/ProjectorDto" },
                    { "#namespace.WhiteboardDto", "#/components/schemas/WhiteboardDto" }
                }
            };
            schema.Required.Add("@odata.type");
            schema.Properties["@odata.type"] = new OpenApiSchema { Type = "string" };
        }
        else if (context.Type == typeof(LearningComponentNoIdDto))
        {
            schema.Discriminator = new OpenApiDiscriminator
            {
                PropertyName = "@odata.type",
                Mapping =
                {
                    { "#namespace.ProjectorNoIdDto", "#/components/schemas/ProjectorNoIdDto" },
                    { "#namespace.WhiteboardNoIdDto", "#/components/schemas/WhiteboardNoIdDto" }
                }
            };
            schema.Required.Add("@odata.type");
            schema.Properties["@odata.type"] = new OpenApiSchema { Type = "string" };
        }
    }
}
