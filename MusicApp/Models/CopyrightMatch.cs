using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusicApp.Models;

public partial class CopyrightMatch
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("track_id")]
    public int TrackId { get; set; }

    [Column("matched_track_id")]
    public int MatchedTrackId { get; set; }

    [Column("similarity_percentage", TypeName = "decimal(5, 2)")]
    public decimal SimilarityPercentage { get; set; }

    [Column("detected_at", TypeName = "datetime")]
    public DateTime? DetectedAt { get; set; }

    [ForeignKey("MatchedTrackId")]
    [InverseProperty("CopyrightMatchMatchedTracks")]
    public virtual Track MatchedTrack { get; set; }

    [ForeignKey("TrackId")]
    [InverseProperty("CopyrightMatchTracks")]
    public virtual Track Track { get; set; }
}
