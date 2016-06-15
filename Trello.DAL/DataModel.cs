namespace Trello.DAL.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=DataModel1")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Board> Boards { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<CardLabel> CardLabels { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentReply> CommentReplies { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<TaskItem> TaskItems { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<UserBoard> UserBoards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Boards)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.CommentReplies)
                .WithRequired(e => e.AspNetUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.UserBoards)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Attachment>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<Board>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Board>()
                .HasMany(e => e.Tickets)
                .WithRequired(e => e.Board)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Board>()
                .HasMany(e => e.UserBoards)
                .WithRequired(e => e.Board)
                .HasForeignKey(e => e.BoardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Board>()
                .HasMany(e => e.UserBoards1)
                .WithRequired(e => e.Board1)
                .HasForeignKey(e => e.BoardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Card>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Card>()
                .Property(e => e.DueDate);
                //.IsUnicode(false);

            modelBuilder.Entity<Card>()
                .Property(e => e.Attachement)
                .IsFixedLength();

            modelBuilder.Entity<Card>()
                .HasMany(e => e.CardLabels)
                .WithRequired(e => e.Card)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(e => e.TaskItems)
                .WithRequired(e => e.Card)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Card)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.Text)
                .IsUnicode(false);

            modelBuilder.Entity<Label>()
                .HasMany(e => e.CardLabels)
                .WithRequired(e => e.Label)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskItem>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TaskItem>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Ticket>()
                .HasMany(e => e.Cards)
                .WithRequired(e => e.Ticket)
                .WillCascadeOnDelete(false);
        }
    }
}
