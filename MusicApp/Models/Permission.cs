using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusicApp.Models;

public partial class Permission
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("requesting_artist_id")]
    [StringLength(450)]
    public string RequestingArtistId { get; set; }

    [Column("track_id")]
    public int TrackId { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; }

    [Column("request_date", TypeName = "datetime")]
    public DateTime? RequestDate { get; set; }

    [ForeignKey("RequestingArtistId")]
    [InverseProperty("Permissions")]
    public virtual User RequestingArtist { get; set; }

    [ForeignKey("TrackId")]
    [InverseProperty("Permissions")]
    public virtual Track Track { get; set; }
}
