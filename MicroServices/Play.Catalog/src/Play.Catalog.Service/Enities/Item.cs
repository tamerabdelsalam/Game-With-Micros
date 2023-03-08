using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Enities;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description{ get; set; }    
    public decimal Price{ get; set; }    
    public DateTimeOffset CreatedOn{ get; set; }    
}
