using System;
using backend.contracts;
using backend.data;
using backend.mapping;
using backend.models;
using backend.services;
using Microsoft.EntityFrameworkCore;

namespace backend.endpoints;

public static class ChatEndpoints
{
  public static RouteGroupBuilder MapChatEndpoints(this WebApplication app)
  {
    var router = app.MapGroup("api/")
                   .WithParameterValidation();

    //Display Conversation History as a List ordered by latest first              
    router.MapGet("conversations/", async (AppDbContext _context) =>
    {
      var conversations = await _context.Conversations
                                  .OrderByDescending(c => c.CreatedAt)
                                  .ToListAsync();

      var dtoList = conversations.Select(c => c.ToConversationRequest()).ToList();
      return Results.Ok(dtoList);
    });


    //Create a new conversation and attach it to its own session
    router.MapPost("conversations/", async (AppDbContext _context) =>
    {
      var conversation = new Conversation();

      var senderId = Guid.NewGuid();

      _context.Conversations.Add(conversation);

      _context.Sessions.Add(new RasaSession
      {
        ConversationId = conversation.ConversationId,
        SenderId = senderId
      });

      await _context.SaveChangesAsync();

      return Results.Ok(conversation);
    });

    router.MapDelete("conversations/{conversationId}", async (Guid conversationId, AppDbContext _context) =>
    {
      var conversation = await _context.Conversations.FindAsync(conversationId);

      if (conversation == null)
        return Results.NotFound("Conversation not found");

      _context.Conversations.Remove(conversation);
      await _context.SaveChangesAsync();

      return Results.Ok(conversation);
    });



    //Get the mesages in a given conversation
    router.MapGet("messages/{conversationId}", async (Guid conversationId, AppDbContext _context) =>
    {
      var messages = await _context.Messages
          .Where(m => m.ConversationId == conversationId)
          .OrderBy(m => m.Timestamp)
          .ToListAsync();

      return Results.Ok(messages);
    });

    router.MapPost("chat/send/", async (ChatRequest request, AppDbContext _context, RasaService rasa) =>
    {
      var conversation = await _context.Conversations
          .FindAsync(request.ConversationId);

      if (conversation == null)
        return Results.NotFound("Conversation not found");

      var session = await _context.Sessions
          .FirstAsync(s => s.ConversationId == request.ConversationId);

      _context.Messages.Add(new Message
      {
        ConversationId = request.ConversationId,
        Sender = "User",
        Text = request.Message
      });

      if (conversation.Title == "New Chat")
      {
        conversation.Title = request.Message.Length > 30
            ? request.Message.Substring(0, 30)
            : request.Message;
      }

      await _context.SaveChangesAsync();

      var botReplies = await rasa.SendMessage(
          session.SenderId.ToString(),
          request.Message
      );

      foreach (var reply in botReplies)
      {
        _context.Messages.Add(new Message
        {
          ConversationId = request.ConversationId,
          Sender = "Bot",
          Text = reply
        });
      }

      await _context.SaveChangesAsync();

      return Results.Ok(botReplies);
    });

    return router;
  }
}
