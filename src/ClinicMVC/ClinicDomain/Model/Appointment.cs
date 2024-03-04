using System;
using System.Collections.Generic;

namespace ClinicDomain.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int PatientId { get; set; }

    public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } = new List<AppointmentProcedure>();

    public virtual PatientCard Patient { get; set; } = null!;
}
