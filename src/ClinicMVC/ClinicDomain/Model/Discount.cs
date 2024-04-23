using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicDomain.Model;

public partial class Discount
{
    public int Id { get; set; }
    [Display(Name = "Соціальна група")]
    public string SocialGroup { get; set; } = null!;
    [Display(Name = "Знижка")]
    public int DiscountPercent { get; set; }

    public virtual ICollection<PatientCard> PatientCards { get; set; } = new List<PatientCard>();
}
