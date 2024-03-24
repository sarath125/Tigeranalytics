using System;
using System.Collections.Generic;

namespace RetailPrice.Models;

public partial class Product
{
    public int Sku { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ICollection<Pricing> Pricings { get; set; } = new List<Pricing>();
}
