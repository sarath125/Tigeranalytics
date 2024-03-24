using System;
using System.Collections.Generic;

namespace RetailPrice.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? ZipCode { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Pricing> Pricings { get; set; } = new List<Pricing>();
}
