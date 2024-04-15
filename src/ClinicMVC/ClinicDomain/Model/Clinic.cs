using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicDomain.Model;

public partial class Clinic
{
    public int Id { get; set; }

    [Display(Name = "Назва клініки")]
    public string Name { get; set; } = null!;

    [Display(Name = "Спеціалізація")]
    public string Specialization { get; set; } = null!;

    [Display(Name = "Фізична адреса")]
    public string Address { get; set; } = null!;

    [Display(Name = "Номер телефону")]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Електронна пошта")]
    public string? Email { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<PatientCard> PatientCards { get; set; } = new List<PatientCard>();

    public virtual ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();
}
