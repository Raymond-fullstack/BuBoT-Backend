using System;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Conversation> Conversations => Set<Conversation>();
  public DbSet<Message> Messages => Set<Message>();
  public DbSet<RasaSession> Sessions => Set<RasaSession>();
}
