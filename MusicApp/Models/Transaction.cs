using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusicApp.Models;

public partial class Transaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("payer_id")]
    [StringLength(450)]
    public string PayerId { get; set; } = null!;

    [Column("receiver_id")]
    [StringLength(450)]
    public string ReceiverId { get; set; } = null!;

    [Column("track_id")]
    public int TrackId { get; set; }

    [Column("amount", TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("payment_date", TypeName = "datetime")]
    public DateTime? PaymentDate { get; set; }

    [ForeignKey("PayerId")]
    [InverseProperty("TransactionPayers")]
    public virtual User Payer { get; set; } = null!;

    [ForeignKey("ReceiverId")]
    [InverseProperty("TransactionReceivers")]
    public virtual User Receiver { get; set; } = null!;

    [ForeignKey("TrackId")]
    [InverseProperty("Transactions")]
    public virtual Track Track { get; set; } = null!;
}
