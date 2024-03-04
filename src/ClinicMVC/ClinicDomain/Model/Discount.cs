﻿using System;
using System.Collections.Generic;

namespace ClinicDomain.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string SocialGroup { get; set; } = null!;

    public int DiscountPercent { get; set; }

    public virtual ICollection<PatientCard> PatientCards { get; set; } = new List<PatientCard>();
}
