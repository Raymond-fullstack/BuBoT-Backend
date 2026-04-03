using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.models;

public class Message
{
  public Guid MessageId { get; set; }

  [ForeignKey("Conversation")]
  public Guid ConversationId { get; set; }
  public string Sender { get; set; } = "User";

  [StringLength(255, ErrorMessage = "Message cannot be longer than 255 characters.")]
  public string Text { get; set; } = string.Empty;
  public DateTime Timestamp { get; set; } = DateTime.UtcNow;

}
