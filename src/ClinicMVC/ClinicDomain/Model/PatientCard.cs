﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicDomain.Models;

public partial class PatientCard
{
    [Display(Name ="№ картки")]
    public int Id { get; set; }

    [Display(Name = "Ім'я")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Прізвище")]
    public string LastName { get; set; } = null!;

    [Display(Name = "По-батькові")]
    public string FatherName { get; set; } = null!;

    [Display(Name = "Номер телефону")]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Дата народження")]
    public DateOnly DateOfBirth { get; set; }

    [Display(Name = "Додаткова інформація")]
    public string? AddInfo { get; set; }

    [Display(Name = "Алергії")]
    public string? Allergy { get; set; }

    [Display(Name = "Хронічні хвороби")]
    public string? ChronicDisease { get; set; }
    
    [Display(Name = "Захворювання")]
    public string? Diseases { get; set; }

    [Display(Name = "Соціальна група")]
    public int DiscountId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [Display(Name = "Знижка")]
    public virtual Discount Discount { get; set; } = null!;
}
