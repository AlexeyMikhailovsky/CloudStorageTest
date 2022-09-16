namespace CloudStorage.WebAPI.DataSource
{
    public class SQLQueries
    {
        public const string ConnectionString = "Data Source=cloudstroragedb.db;Mode=ReadWrite;";
        public const string GetFileSQL = "SELECT * FROM files WHERE id = @id;";
        public const string GetFilesFromFolderSQL = "SELECT * FROM files WHERE id_folder = @id;";
        public const string GetAllFilesSQL = "SELECT * FROM files;";
        public const string DeleteFileSQL = "DELETE FROM files WHERE id = @id;";
        public const string UploadFileSQL = "INSERT INTO files(id_folder, location, name, creation_date, upload_date, type, size)" +
                            "VALUES (@id, @location, @name, @creation_date, @upload_date, @type, @size);";
        public const string GetFolderSQL = "SELECT * FROM folders WHERE id_folder = @id;";
        public const string GetAllFoldersSQL = "SELECT * FROM folderss;";
        public const string DeleteFolderSQL = "DELETE FROM folders WHERE id_folder = @id;";
        public const string AddFolderSQL = "INSERT INTO folders (id_parent, name) VALUES (@id_parent,@name);";
        public const string UpdateFolderSQL = "UPDATE folders SET name = @name WHERE id_folder = @id;";
        public const string GetAllFoldersAndFilesSQL = "SELECT * FROM folders LEFT JOIN files on folders.id_folder = files.id_folder;";
        public const string GetAllFoldersIDSQL = "SELECT id_folder FROM folders WHERE id_parent = @id";
    }
}
