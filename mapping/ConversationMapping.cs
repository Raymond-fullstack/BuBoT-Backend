using System;
using backend.contracts;
using backend.models;

namespace backend.mapping;

public static class ConversationMapping
{
  public static ConversationRequest ToConversationRequest(this Conversation conversation)
  {
    return new(
      conversation.ConversationId,
      conversation.Title,
      conversation.CreatedAt
    );
  }
}
