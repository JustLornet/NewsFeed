using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace NewsFeedEngine.Domain.Aggregates
{
    public class Post : EntityBase, IEntityTypeConfiguration<Post>
    {
        public long UserId { get; set; }

        public User User { get; set; }

        public DateTime CreateAt { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasOne(p => p.User).WithMany(p => p.Posts).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasData(new Post
            {
                Id = 1,
                UserId = 1,
                CreateAt = default(DateTime),
                Content = "test1",
                ElementStatusId = 1,
            },
            new Post
            {
                Id = 2,
                UserId = 1,
                CreateAt = default(DateTime),
                Content = "test2",
                ElementStatusId = 1,
            },
            new Post
            {
                Id = 3,
                UserId = 2,
                CreateAt = default(DateTime),
                Content = "test3",
                ElementStatusId = 1,
            },
            new Post
            {
                Id = 4,
                UserId = 3,
                CreateAt = default(DateTime),
                Content = "test3",
                ElementStatusId = 1,
            });
        }
    }
}
