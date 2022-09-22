using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WebAPIKurs.Models;

namespace WebAPIKurs.Formatters
{
    public class VCardInputFormatter : TextInputFormatter
    {

        public VCardInputFormatter()
        {
            //Properties der Basisklasse TextInputFormatter wird initialisiert
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            HttpContext httpContext = context.HttpContext;
            IServiceProvider serviceProvider = httpContext.RequestServices;
            ILogger logger = serviceProvider.GetRequiredService<ILogger<InputFormatterResult>>();

            using var reader = new StreamReader(httpContext.Request.Body, encoding); //using gilt bis Ende der Methode -> Dispose wird aufgerufen

            string nameLine = null;
            string idLine = null;

            try
            {
                await ReadLineAsync("BEGIN:VCARD", reader, context, logger);
                await ReadLineAsync("VERSION:", reader, context, logger);

                nameLine = await ReadLineAsync("N:", reader, context, logger);

                var split = nameLine.Split(";".ToCharArray());

                var contact = new Contact
                {
                    Lastname = split[0].Substring(2),
                    Firstname = split[1]
                };

                await ReadLineAsync("FN:", reader, context, logger);
                idLine = await ReadLineAsync("UID:", reader, context, logger);
                var splitId = idLine.Split(":");
                contact.Id = Convert.ToInt32(splitId[1]);
                await ReadLineAsync("END:VCARD", reader, context, logger);

                logger.LogInformation("nameLine = {nameLine}", nameLine);

                return await InputFormatterResult.SuccessAsync(contact);
            }
            catch
            {
                logger.LogError("Read failed: nameLine = {nameLine}", nameLine);
                return await InputFormatterResult.FailureAsync();
            }

        }

        private static async Task<string> ReadLineAsync(
            string expectedText, StreamReader reader, InputFormatterContext context,
            ILogger logger)
        {
            string line = await reader.ReadLineAsync();

            if (!line.StartsWith(expectedText))
            {
                var errorMessage = $"Looked for '{expectedText}' and got '{line}'";

                context.ModelState.TryAddModelError(context.ModelName, errorMessage);
                logger.LogError(errorMessage);

                throw new Exception(errorMessage);
            }

            return line;
        }
    }
}
