using System.Collections.Generic;

namespace Cube.Model.Model.Interfaces
{
    /// <summary>
    /// Объект с изображением.
    /// </summary>
    public interface IImageHolder
    {
        /// <summary>
        /// Изображения.
        /// </summary>
        ICollection<Image> Images { get; }
    }
}