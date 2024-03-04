using System;
using System.Collections.Generic;

namespace ClinicDomain.Models;

public partial class User
{
    public int Id { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public int ClinicId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } = new List<AppointmentProcedure>();

    public virtual Clinic Clinic { get; set; } = null!;
}
