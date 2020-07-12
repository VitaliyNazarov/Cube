using System.Collections.Generic;

namespace Cube.Model.Interfaces
{
    /// <summary>
    /// Объект с изображением.
    /// </summary>
    public interface IImageHolder
    {
        /// <summary>
        /// Изображения.
        /// </summary>
        IList<Image> Images { get; }
    }
}