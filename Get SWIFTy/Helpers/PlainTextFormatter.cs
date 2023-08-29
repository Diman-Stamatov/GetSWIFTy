using Microsoft.AspNetCore.Mvc.Formatters;

namespace Get_SWIFTy.Helpers;

public class PlainTextFormatter : InputFormatter
{
    private const string ContentType = "text/plain";

    public PlainTextFormatter()
    {
        SupportedMediaTypes.Add(ContentType);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        var request = context.HttpContext.Request;
        using (var reader = new StreamReader(request.Body))
        {
            var content = await reader.ReadToEndAsync();
            return await InputFormatterResult.SuccessAsync(content);
        }
    }

    public override bool CanRead(InputFormatterContext context)
    {
        var contentType = context.HttpContext.Request.ContentType;
        return contentType.StartsWith(ContentType);
    }
}
