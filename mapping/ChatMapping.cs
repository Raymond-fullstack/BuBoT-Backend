using System;
using backend.contracts;
using backend.models;

namespace backend.mapping;

public static class ChatMapping
{
  public static ChatRequest ToChatRequest(this Message message)
  {
    return new(
      message.ConversationId,
      message.Text
    );
  }
}
