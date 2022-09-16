namespace CloudStorage.WebAPI.Model
{
    public class MyFile
    {
        public int Id { get; set; }

        public int IdFolder { get; set; }

        public string Location { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string CreationDate { get; set; } = null!;

        public string UploadDate { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Size { get; set; } = null!;
    }
}
