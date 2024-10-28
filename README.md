# PackageFunctionApp
This repository contains a description and implementation of the programming task used to evaluate software developers.


## Task
Alongside this challenge, you will also find a [sample data file](data.txt), which is a fixed-length text file.

This is a sample of an ASN (Acknowledgement Shipping Notification) message that we receive from one of our suppliers.
Each HDR section, represents a box, and the lines below the HDR section describe the contents of the box.
When we reach another HDR section, it means that there is another box and we repeat the process from the beginning.

## Data file structure
<pre>
HDR  TRSP117                                           6874454I                           
LINE P000001661         9781465121550         12     
LINE P000001661         9925151267712         2      
LINE P000001661         9651216865465         1      
</pre>

## Description
<pre>
HDR             - Just a keyword telling that a new box is being described.
TRSP117         - Supplier identifier.
6874454I        - Carton box identifier. Displayed on the box.
LINE            - Keyword to identify product item in the box.
P000001661      - Our PO Number that we sent to the supplier.
9781465121550   - ISBN 13 (product barcode).
12              - Product quantity.
</pre>

The solution should monitor a specific file path, and whenever a file is dropped in that folder, the file should be parsed and loaded into a database.
The file could be very large and exceed the available RAM.

IMPORTANT: Your submission will be verified for correctness, and also used to evaluate your approach, attention to detail, and craftmanship. 
This is your opportunity to give us an idea of what we can expect from you on a day-to-day basis. Try to write the code as you would any ticket assigned to you. 
We understand this is an effort you do in your free time, and we do not expect a fully polished production ready product. It's perfectly fine to take shortcuts and omit code 
in case of time contraints. Just drop a comment where you would have implemented code, and explain what approach you would take and what the code would have done.

Because it's an assignment for a senior position, we'd ask you to implement at least one advanced approach in the code, such as memory optimization, efficient bulk insertion strategies, or similar.

    The following code class is merely an example to demonstrate the structure. You should modify it as needed to achieve your goal.
    If you have any question about the task, please feel free to ask.

```csharp
public class Box
{
    public string SupplierIdentifier { get; set; }
    public string Identifier { get; set; }

    public IReadOnlyCollection<Content> Contents { get; set; } 

    public class Content
    {
        public string PoNumber { get; set; }
        public string Isbn { get; set; }
        public int Quantity { get; set; }
    }
}
```
We look forward to seeing your solution!

## Running project locally

This project uses SQLite as database engine and Azurite as Azure storage emulator to run timed functions.

Run default azurite container in Docker:

```
docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 mcr.microsoft.com/azure-storage/azurite
```


Fill required appsettings for abstract shared ftp locations:

```
"FolderStructure": {
        "Shared": "",
        "Processing": "",
        "Processed": ""
```

Install the [Azure Functions Core Tools](https://go.microsoft.com/fwlink/?linkid=2174087) and run project.
