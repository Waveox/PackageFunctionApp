using PackageFunctionApp.App.Models.Base;

namespace PackageFunctionApp.App.Models;

public sealed class Item : ITimestamped, IEntity<Guid>
{
    public Guid Id { get; set; }
    public string PoNumber { get; set; }
    public string Barcode { get; set; }
    public int Quantity { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}