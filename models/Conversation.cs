using System;

namespace backend.models;

public class Conversation
{
  public Guid ConversationId { get; set; }
  public string Title { get; set; } = "New Chat";
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public List<Message> Messages { get; set; } = new();

}
