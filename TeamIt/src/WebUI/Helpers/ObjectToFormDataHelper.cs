using Blazorise;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace WebUI.Helpers
{
    public static class ObjectToFormDataHelper
    {
        public static MultipartFormDataContent ToFormData(this object source, IFileEntry? image = null)
        {
            var content = new MultipartFormDataContent();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                object? value = property.GetValue(source);
                switch (value)
                {
                    case null when property.PropertyType != typeof(IFormFile):
                        content.Add(new StringContent(""), property.Name);
                        break;
                    case null when property.PropertyType == typeof(IFormFile):
                        if (image is null)
                            content.Add(new StringContent(""), property.Name);
                        else
                        {
                            var fileContent = new StreamContent(image.OpenReadStream(long.MaxValue));
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.Type);
                            content.Add(fileContent, property.Name, image.Name);
                        }
                        break;
                    case IEnumerable list and not string:
                        foreach (var item in list)
                            content.Add(new StringContent(item.ToString() ?? ""), property.Name);
                        break;
                    default:
                        content.Add(new StringContent(value!.ToString() ?? ""), property.Name);
                        break;
                }
            }
            return content;
        }
    }
}
