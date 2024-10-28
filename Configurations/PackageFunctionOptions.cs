namespace PackageFunctionApp.Configurations;
public class PackageFunctionOptions
{
    public DelimetersOptions Delimeters { get; set; }
    public FolderStructureOptions FolderStructure { get; set; }

     public FilesOptions Files { get; set; }

    public class DelimetersOptions
    {
        public string Package { get; set; }
        public string Item { get; set; }
    }

    public class FilesOptions
    {
        public string AllowedExtensions { get; set; }
        public string LockFileExtension { get; set; }
    }

    public class FolderStructureOptions
    {
        public string Shared { get; set; }
        public string Processing { get; set; }
        public string Processed { get; set; }
    }
}