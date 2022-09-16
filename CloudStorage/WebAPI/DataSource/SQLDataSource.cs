using CloudStorage.Controllers;
using CloudStorage.WebAPI.Model;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Xml.Linq;
using static CloudStorage.WebAPI.DataSource.SQLQueries;

namespace CloudStorage.WebAPI.DataSource
{
    public class SQLDataSource : IDataSource
    {
        SqliteConnection myConnection = new SqliteConnection(ConnectionString);
        private readonly ILogger<SQLDataSource> _logger;
        public SQLDataSource(ILogger<SQLDataSource> logger)
        {
            _logger = logger;
        }

        public SQLDataSource()
        {
        }

        public bool CreateFolder(int id, string name)
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(AddFolderSQL, myConnection);
                cmd.Parameters.AddWithValue("@id_parent", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
            finally 
            { 
                myConnection.Close(); 
            }
        }

        public bool DeleteFile(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(DeleteFileSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public bool DeleteFolder(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(DeleteFolderSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public MyFile GetFile(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(GetFileSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                MyFile file = new MyFile();
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            file.Id = reader.GetInt32(0);
                            file.IdFolder = reader.GetInt32(1);
                            file.Location = reader.GetString(2);
                            file.Name = reader.GetString(3);
                            file.CreationDate = reader.GetString(4);
                            file.UploadDate = reader.GetString(5);
                            file.Type = reader.GetString(6);
                            file.Size = reader.GetString(7);
                        }
                    }
                    else 
                    {
                        return null;
                    }
                }
                return file;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public List<MyFile> GetFiles()
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(GetAllFilesSQL, myConnection);
                cmd.Prepare();
                List<MyFile> files = new List<MyFile>();    
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MyFile file = new MyFile();
                            file.Id = reader.GetInt32(0);
                            file.IdFolder = reader.GetInt32(1);
                            file.Location = reader.GetString(2);
                            file.Name = reader.GetString(3);
                            file.CreationDate = reader.GetString(4);
                            file.UploadDate = reader.GetString(5);
                            file.Type = reader.GetString(6);
                            file.Size = reader.GetString(7);
                            files.Add(file);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public Folder GetFolder(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(GetFolderSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                Folder folder = new Folder();
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            folder.Id = reader.GetInt32(0);
                            folder.IdParent = reader.GetInt32(1);
                            folder.Name = reader.GetString(2);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return folder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public bool UpdateFolder(int id, string name)
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(UpdateFolderSQL, myConnection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public bool UploadFile(MyFile myFile)
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(UploadFileSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", myFile.IdFolder);
                cmd.Parameters.AddWithValue("@location", myFile.Location);
                cmd.Parameters.AddWithValue("@name", myFile.Name);
                cmd.Parameters.AddWithValue("@creation_date", myFile.CreationDate);
                cmd.Parameters.AddWithValue("@upload_date", myFile.UploadDate);
                cmd.Parameters.AddWithValue("@type", myFile.Type);
                cmd.Parameters.AddWithValue("@size", myFile.Size);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public List<MyFile> GetFilesFromFolder(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new SqliteCommand(GetFilesFromFolderSQL, myConnection);
                cmd.Parameters.AddWithValue("@id_folder", id);
                cmd.Prepare();
                List<MyFile> files = new List<MyFile>();
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MyFile file = new MyFile();
                            file.Id = reader.GetInt32(0);
                            file.IdFolder = reader.GetInt32(1);
                            file.Location = reader.GetString(2);
                            file.Name = reader.GetString(3);
                            file.CreationDate = reader.GetString(4);
                            file.UploadDate = reader.GetString(5);
                            file.Type = reader.GetString(6);
                            file.Size = reader.GetString(7);
                            files.Add(file);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
            finally
            {
                myConnection.Close();
            }
        }
    }
}
