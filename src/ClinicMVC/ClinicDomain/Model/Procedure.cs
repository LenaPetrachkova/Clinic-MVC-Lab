using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicDomain.Models;

public partial class Procedure
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Display(Name = "Вартість")]
    public double Price { get; set; }

    public int ClinicId { get; set; }

    public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } = new List<AppointmentProcedure>();

    public virtual Clinic Clinic { get; set; } = null!;
}
