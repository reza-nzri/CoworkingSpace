﻿using System;
using System.Collections.Generic;

namespace CoworkingSpaceAPI.Models;

public partial class Label
{
    public int LabelId { get; set; }

    public string LabelName { get; set; } = null!;

    public string? Description { get; set; }

    public string? ColorCode { get; set; }

    public virtual ICollection<LabelAssignment> LabelAssignments { get; set; } = new List<LabelAssignment>();
}
