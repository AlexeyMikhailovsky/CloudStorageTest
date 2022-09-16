using CloudStorage.WebAPI.Model;

namespace CloudStorage.WebAPI.DataSource
{
    public interface IDataSource
    {
        MyFile GetFile(int id);
        List<MyFile> GetFiles();
        bool DeleteFile(int id);
        List<MyFile> GetFilesFromFolder(int id);
        bool UploadFile(MyFile myFile);
        bool CreateFolder(int id, string name);
        Folder GetFolder(int id);
        bool DeleteFolder(int id);
        bool UpdateFolder(int id, string name);
    }
}
