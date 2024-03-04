using System;
using System.Collections.Generic;

namespace ClinicDomain.Models;

public partial class Procedure
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public int ClinicId { get; set; }

    public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } = new List<AppointmentProcedure>();

    public virtual Clinic Clinic { get; set; } = null!;
}
