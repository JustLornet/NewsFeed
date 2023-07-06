using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsFeedEngine.Domain.Aggregates
{
    public class User : EntityBase, IEntityTypeConfiguration<User>
    {
        // имя
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        // фамилия
        [Required(AllowEmptyStrings = false)]
        public string Surname { get; set; }

        // отчество
        public string? Patronymic { get; set; }

        public List<Post> Posts { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                var isPatronExists = !string.IsNullOrWhiteSpace(Patronymic);
                var actualPatronymic = isPatronExists ? $" {Patronymic}" : string.Empty;

                return $"{Surname} {FirstName}{actualPatronymic}";
            }
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                Id = 1,
                FirstName = "test",
                Surname = "test",
                Patronymic = "test",
            },
            new User
            {
                Id = 2,
                FirstName = "test",
                Surname = "test",
                Patronymic = "test",
            },
            new User
            {
                Id = 3,
                FirstName = "test",
                Surname = "test",
                Patronymic = "test",
            });
        }
    }
}
