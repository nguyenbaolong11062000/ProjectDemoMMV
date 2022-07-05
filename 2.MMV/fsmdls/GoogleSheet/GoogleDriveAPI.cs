using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace MMV.fsmdls.GoogleSheet
{
    public class GoogleDriveAPI
    {
        public static string[] Scopes = { Google.Apis.Drive.v3.DriveService.Scope.Drive };

        //create Drive API service.
        public static DriveService GetService()
        {
            //get Credentials from client_secret.json file 
            UserCredential credential;
            //Root Folder of project
            var CSPath = System.Web.Hosting.HostingEnvironment.MapPath("~/fsmdls/GoogleSheet");
            //credentials.json thiết lập xác thực tài khoản google
            using (var stream = new FileStream(Path.Combine(CSPath, "credentials.json"), FileMode.Open, FileAccess.Read))
            {
                String FolderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/fsmdls/GoogleSheet/"); ;
                //File json lưu thông tin tài khoản upload file, xóa file này đi nếu muốn đổi tài khoản upload khác
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }
            //create Drive API service.
            DriveService service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveUpload",
            });
            return service;
        }


        //file Upload to the Google Drive root folder.
        //public static string UploadFileOnDrive(string fileName,string path)
        //{

        //        //create service
        //        DriveService service = GetService();
        //        //File backup
        //        var FileMetaData = new Google.Apis.Drive.v3.Data.File();
        //        FileMetaData.Name = Path.GetFileName(fileName);
        //        FileMetaData.MimeType = MimeMapping.GetMimeMapping(path);
        //        FileMetaData.Parents= new List<string>();
        //        //ID thư mục chứa file upload
        //        FileMetaData.Parents.Add("159tRN0PN0vdTjznkjJfneSaknXqlb-9o");
        //        FilesResource.CreateMediaUpload request;
        //        using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
        //        {
        //            request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
        //            request.Fields = "id";
        //            request.Upload();
        //        }
        //        var _fileID = request.ResponseBody;
        //        return _fileID.Id;

        //}

        public static string UploadFileOnDrive(HttpPostedFile file,string key)
        {
            if (file != null && file.ContentLength > 0)
            {
                //create service
                DriveService service = GetService();
                //File backup
                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/fsmdls/GoogleSheet/Upload"),
                Path.GetFileName(file.FileName));
                file.SaveAs(path);
                var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                FileMetaData.Name = Path.GetFileName(file.FileName);
                FileMetaData.MimeType = MimeMapping.GetMimeMapping(path);
                FileMetaData.Parents = new List<string>();
                //ID thư mục chứa file upload
                FileMetaData.Parents.Add(key);
                FilesResource.CreateMediaUpload request;
                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                {
                    request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                    request.Fields = "id";
                    request.Upload();
                }
                var _fileID = request.ResponseBody;
                return _fileID.Id;
            }
            return null;

        }
    }
}