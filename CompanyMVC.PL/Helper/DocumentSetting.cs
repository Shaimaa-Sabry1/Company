namespace CompanyMVC.PL.Helper
{
    public static class DocumentSetting
    {
        //Upload
        //returen ImageName (Unique Name)
        public static string UploadFile(IFormFile file,string folderName)
        {
            //1. Get Folder Location
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folderName);
            //2. file Name Use Guid To Make File Name Unique
            var fileName = $"{Guid.NewGuid()}{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);
            //file Path
            var fileStream = new FileStream(filePath,FileMode.Create);
            file.CopyTo(fileStream);
            return fileName;
        }
        //Delete
        public static void Delete(string fileName,string folderName)

        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName,fileName);
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }

    }
}
