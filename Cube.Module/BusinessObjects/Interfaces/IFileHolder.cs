using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl.EF;

namespace Cube.Model.Interfaces
{
    /// <summary>
    /// Объект с файлами.
    /// </summary>
    public interface IFileHolder
    {
        /// <summary>
        /// Файлы.
        /// </summary>
        IList<FileData> Files { get; }
    }
}