using System;
using System.Collections.Generic;

namespace SchoolSystemAB.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public int EmployeeId { get; set; }

    public int ClassId { get; set; }

    public string Grade1 { get; set; } = null!;

    public DateOnly GradeDate { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
