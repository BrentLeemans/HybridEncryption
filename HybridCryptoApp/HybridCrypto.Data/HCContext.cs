using HybridCrypto.Business;
using HybridCrypto.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HybridCrypto.Data
{
    public class HCContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Receiver> Receivers { get; set; }
        public DbSet<Sender> Senders { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<EncryptedPacket> EncryptedPackets { get; set; } = null!;


        public HCContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(r => r.Messages)
                .HasForeignKey(m => m.ReceiverId);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(s => s.Messages)
                .HasForeignKey(m => m.SenderId);

            builder.Entity<EncryptedPacket>()
                 .HasOne(e => e.Message)
                 .WithMany(m => m.EncryptedPackets)
                 .HasForeignKey(e => e.MessageId);
        }
    }
}
