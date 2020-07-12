using System;

namespace Cube.Model.Model.Interfaces
{
    /// <summary>
    /// Контракт объектов поддерживающих мягкое удаление.
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Дата удаления группы.
        /// </summary>
        DateTime? DeletedDate { get; }

        /// <summary>
        /// Удаленная группа.
        /// </summary>
        bool Deleted { get; }
    }
}