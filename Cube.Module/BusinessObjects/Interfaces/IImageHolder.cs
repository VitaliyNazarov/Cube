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
        //Image Image { get; }

        byte[] Image { get; }
    }
}