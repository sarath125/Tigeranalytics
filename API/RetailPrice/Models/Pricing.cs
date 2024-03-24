using System;
using System.Collections.Generic;

namespace RetailPrice.Models;

public partial class Pricing
{
    public int PricingId { get; set; }

    public int StoreId { get; set; }

    public int Sku { get; set; }

    public decimal Price { get; set; }

    public DateTime? Date { get; set; }

    public virtual Product SkuNavigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}

