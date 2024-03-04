using System;
using System.Collections.Generic;

namespace ClinicDomain.Models;

public partial class AppointmentProcedure
{
    public int Id { get; set; }

    public int AppointmentId { get; set; }

    public int ProceduresId { get; set; }

    public int UserId { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Procedure Procedures { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
