using System;
using System.Collections.Generic;

namespace SchoolSystemAB.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string UserType { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }
}
