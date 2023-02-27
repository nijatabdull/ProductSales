using System.Text.RegularExpressions;

namespace ProductSale.Service.Catalog.Configurations.Helpers
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            //transform string to kebab-case
            return value == null ? null : Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
