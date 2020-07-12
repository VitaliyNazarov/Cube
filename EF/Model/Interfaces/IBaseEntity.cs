using System;

namespace Cube.Model.Model.Interfaces
{
    /// <summary>
    /// Контракт объектов с идентификацией.
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Внешний уникальный идентификатор.
        /// (нужен если будут сливаться несколько баз в одну)
        /// </summary>
        Guid ExternalId { get; }

        /// <summary>
        /// Дата создания объекта.
        /// </summary>
        DateTime CreatedDate { get; }
    }
}