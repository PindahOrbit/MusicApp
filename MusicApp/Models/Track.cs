using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusicApp.Models;

public partial class Track
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    [StringLength(450)]
    public string UserId { get; set; } = null!;

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("genre")]
    [StringLength(100)]
    public string? Genre { get; set; }

    [Column("release_date")]
    public DateOnly? ReleaseDate { get; set; }

    [Column("file_path")]
    [StringLength(500)]
    public string FilePath { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("MatchedTrack")]
    public virtual ICollection<CopyrightMatch> CopyrightMatchMatchedTracks { get; set; } = new List<CopyrightMatch>();

    [InverseProperty("Track")]
    public virtual ICollection<CopyrightMatch> CopyrightMatchTracks { get; set; } = new List<CopyrightMatch>();

    [InverseProperty("Track")]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    [InverseProperty("Track")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [ForeignKey("UserId")]
    [InverseProperty("Tracks")]
    public virtual User User { get; set; } = null!;
}
