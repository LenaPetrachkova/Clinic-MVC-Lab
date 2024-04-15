using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicDomain.Model;

public partial class Appointment
{
    public int Id { get; set; }

    [Display(Name ="Дата")]
    public DateOnly Date { get; set; }

    [Display(Name = "Час початку")]
    public TimeOnly StartTime { get; set; }

    [Display(Name = "Час закінчення")]
    public TimeOnly EndTime { get; set; }

    [Display(Name = "Пацієнт")]
    public int PatientId { get; set; }

    [Display(Name = "Процедура")]
    public int ProceduresId { get; set; }

    [Display(Name = "Доктор")]
    public int DoctorId { get; set; }

    public int? ClinicId { get; set; }

    public virtual Clinic? Clinic { get; set; }

    [Display(Name = "Лікар")]
    public virtual Doctor Doctor { get; set; } = null!;

    [Display(Name = "Пацієнт")]
    public virtual PatientCard Patient { get; set; } = null!;

    [Display(Name = "Процедура")]
    public virtual Procedure Procedures { get; set; } = null!;
}
