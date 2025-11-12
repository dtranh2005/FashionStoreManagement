using System;
using System.Collections.Generic;

namespace FashionStoreManagement.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Category { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = "Active";
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
