﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusicApp.Models;

[Index("NormalizedEmail", Name = "EmailIndex")]
public partial class User
{
    [Key]
    public string Id { get; set; }

    [StringLength(256)]
    public string UserName { get; set; }

    [StringLength(256)]
    public string NormalizedUserName { get; set; }

    [StringLength(256)]
    public string Email { get; set; }

    [StringLength(256)]
    public string NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    [InverseProperty("Receiver")]
    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    [InverseProperty("Sender")]
    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    [InverseProperty("RequestingArtist")]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    [InverseProperty("User")]
    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    [InverseProperty("Payer")]
    public virtual ICollection<Transaction> TransactionPayers { get; set; } = new List<Transaction>();

    [InverseProperty("Receiver")]
    public virtual ICollection<Transaction> TransactionReceivers { get; set; } = new List<Transaction>();

    [InverseProperty("User")]
    public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();

    [InverseProperty("User")]
    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    [InverseProperty("User")]
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
