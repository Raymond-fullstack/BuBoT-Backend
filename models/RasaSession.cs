using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.models;

public class RasaSession
{
  [Key]
  public Guid SessionId { get; set; }

  [ForeignKey("Conversation")]
  public Guid ConversationId { get; set; }
  public Guid SenderId { get; set; }

}
