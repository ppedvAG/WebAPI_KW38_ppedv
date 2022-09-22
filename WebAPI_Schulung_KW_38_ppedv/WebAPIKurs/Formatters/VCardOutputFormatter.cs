using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using WebAPIKurs.Models;

namespace WebAPIKurs.Formatters
{
    //Dieser Formatter wandelt Objekte in VCard um und gibt diese aus
    public class VCardOutputFormatter : Microsoft.AspNetCore.Mvc.Formatters.TextOutputFormatter
    {
        public VCardOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(Contact).IsAssignableFrom(type) || typeof(IEnumerable<Contact>).IsAssignableFrom(type);
        }


        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            HttpContext httpContext = context.HttpContext;
            IServiceProvider serviceProvider = httpContext.RequestServices;

            //Wollen Logger-Funktionalität aus IOC Container 
            ILogger<VCardOutputFormatter> logger = serviceProvider.GetRequiredService<ILogger<VCardOutputFormatter>>();


            StringBuilder sb = new StringBuilder();


            //Wird eine Liste nach außen gegeben
            if (context.Object is IEnumerable<Contact> contacts)
            {
                foreach(Contact contact in contacts)
                {
                    FormatVCard(sb, contact, logger);
                }
            }
            else //Oder nur ein Datensatz
            {
                FormatVCard(sb, (Contact)context.Object, logger);
            }


            //VCARD Struktur wird als Antwort ausgegeben
            await httpContext.Response.WriteAsync(sb.ToString());
        }

        private static void FormatVCard(StringBuilder buffer, Contact contact, ILogger logger)
        {
            buffer.AppendLine("BEGIN:VCARD");
            buffer.AppendLine("VERSION:2.1");
            buffer.AppendLine($"N:{contact.Lastname};{contact.Firstname}");
            buffer.AppendLine($"FN:{contact.Firstname} {contact.Lastname}");
            buffer.AppendLine($"UID:{contact.Id}");
            buffer.AppendLine("END:VCARD");

            logger.LogInformation("Writing {FirstName} {LastName}",
                contact.Firstname, contact.Lastname);
        }
    }
}
