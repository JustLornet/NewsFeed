using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NewsFeedEngine.Domain.Aggregates
{
    /// <summary>
    /// Базовый тип сущности БД
    /// </summary>
    public abstract class EntityBase
    {
        [Key]
        public long Id { get; set; }

        // отображение статуса сущности
        public long? ElementStatusId { get; set; }
        public ElementStatus? ElementStatus { get; set; }
    }
}
