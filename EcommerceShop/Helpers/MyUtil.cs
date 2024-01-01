using System.Text;

namespace EcommerceShop.Helpers
{
    public class MyUtil
    {   

        public static string UpLoadImg(IFormFile Hinh, string folder)
        {   
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, Hinh.FileName);
                using (var myFile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myFile);
                }
                return Hinh.FileName;
            } 
            catch (Exception ex) 
            {
                return string.Empty;
            }
        }

        public static string GenerateRandomKey(int lenght = 5)
        {
            var pattern = @"qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < lenght; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}
