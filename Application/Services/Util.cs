using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Application.Services
{
    public class Util
    {
        private static string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hasehdPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hasehdPassword);
        }

        public string UploadFile(IFormFile file, string name, string virtualPath)
        {
            try
            {
                var fileExtension = string.Empty;
                if (file.ContentType != null)
                    fileExtension = file.ContentType.Split("/")[1];

                else
                {
                    string[] split = file.FileName.Split(".");
                    fileExtension = split[split.Length - 1];
                }
                var filename = name + "." + fileExtension;
                var fullPath = Path.Combine(pathToSave, filename);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return virtualPath + filename;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public string GetFileName(string name)
        {
            return string.Format("{0}_{1}-{2}-{3}_{4}{5}.REM",
                                  name,
                                  DateTime.Now.Year,
                                  DateTime.Now.Month.ToString().PadLeft(2, '0'),
                                  DateTime.Now.Day.ToString().PadLeft(2, '0'),
                                  DateTime.Now.Hour.ToString().PadLeft(2, '0'),
                                  DateTime.Now.Minute.ToString().PadLeft(2, '0'));
        }
    }
}
