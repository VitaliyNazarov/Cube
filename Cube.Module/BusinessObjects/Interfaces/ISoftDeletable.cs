using System;

namespace Cube.Model.Interfaces
{
    /// <summary>
    /// Контракт объектов поддерживающих мягкое удаление.
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Дата удаления группы.
        /// </summary>
        DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Удаленная группа.
        /// </summary>
        bool Deleted { get; set; }
    }
}