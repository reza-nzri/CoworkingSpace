using System;
using System.Collections.Generic;

namespace CoworkingSpaceAPI.Models;

public partial class CompanyAddress
{
    public int CompanyAddressId { get; set; }

    public int? CompanyId { get; set; }

    public int? AddressId { get; set; }

    public int? AddressTypeId { get; set; }

    public bool? IsDefault { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Address? Address { get; set; }

    public virtual AddressType? AddressType { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
