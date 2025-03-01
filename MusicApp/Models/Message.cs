using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusicApp.Models;

public partial class Message
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("sender_id")]
    [StringLength(450)]
    public string SenderId { get; set; }

    [Required]
    [Column("receiver_id")]
    [StringLength(450)]
    public string ReceiverId { get; set; }

    [Required]
    [Column("message")]
    public string Message1 { get; set; }

    [Column("sent_at", TypeName = "datetime")]
    public DateTime? SentAt { get; set; }

    [ForeignKey("ReceiverId")]
    [InverseProperty("MessageReceivers")]
    public virtual User Receiver { get; set; }

    [ForeignKey("SenderId")]
    [InverseProperty("MessageSenders")]
    public virtual User Sender { get; set; }
}
