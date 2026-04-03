namespace backend.contracts;

public record ConversationRequest(
  Guid ConversationId,
  string Title,
  DateTime CreatedAt
);
