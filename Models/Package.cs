using System.ComponentModel.DataAnnotations.Schema;
using PackageFunctionApp.App.Models.Base;

namespace PackageFunctionApp.App.Models;

public sealed class Package: ITimestamped
{
    public string SupplierId { get; set; }
    public string PackageId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public ICollection<Item> Items { get; private set; }
    public Package()
    {
        Items = new List<Item>();
    }
}