namespace Cube.Model.Interfaces
{
    /// <summary>
    /// Контракт объектов поддерживающих архивирование.
    /// </summary>
    public interface IArchivable
    {
        /// <summary>
        /// Архивный элемент.
        /// </summary>
        bool IsArchive { get; }
    }
}