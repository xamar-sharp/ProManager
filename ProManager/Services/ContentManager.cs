using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace ProManager.Services
{
    public sealed class ContentManager : IContentManager
    {
        public byte[] Serialize(object content, out bool isFile)
        {
            if (content is null)
            {
                throw new ArgumentNullException();
            }
            if (content is IFormFile)
            {
                isFile = true;
                return ReadFromStream((content as IFormFile).OpenReadStream());
            }
            else
            {
                isFile = false;
                return Encoding.UTF8.GetBytes(content.ToString());
            }
        }
        public string Deserialize(byte[] data, bool isFile)
        {
            return isFile ? Convert.ToBase64String(data) : Encoding.UTF8.GetString(data);
        }
        public byte[] ReadFromStream(Stream source)
        {
            byte[] bytes = new byte[source.Length];
            source.Read(bytes);
            return bytes;
        }
    }
}
