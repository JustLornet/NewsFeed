using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsFeedEngine.Domain.Aggregates.Service;
using System.ComponentModel.DataAnnotations;

namespace NewsFeedEngine.Domain.Aggregates
{
    /// <summary>
    /// Класс для отображения статуса сущности в БД для проведения тестов
    /// </summary>
    public class ElementStatus : IEntityTypeConfiguration<ElementStatus>
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public void Configure(EntityTypeBuilder<ElementStatus> builder)
        {
            builder.HasData(new ElementStatus
            {
                Id = 1,
                Name = ElementStatusEnum.Standart,
            },
            new ElementStatus
            {
                Id = 2,
                Name = ElementStatusEnum.Private
            },
            new ElementStatus
            {
                Id = 3,
                Name = ElementStatusEnum.Test
            });
        }
    }
}
