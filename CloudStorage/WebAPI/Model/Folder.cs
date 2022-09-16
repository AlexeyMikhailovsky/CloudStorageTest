namespace CloudStorage.WebAPI.Model
{
    public class Folder
    {
        public int Id { get; set; }

        public int IdParent { get; set; }

        public string Name { get; set; } = null!;

    }
}
