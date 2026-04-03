namespace backend.contracts;

public record ChatRequest
(
  Guid ConversationId,
  string Message
);
