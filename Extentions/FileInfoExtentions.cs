using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutofacTutorial.Extentions
{
    public static class FileInfoExtentions
    {
        /// <summary>
        /// позволяет выполнять запуск файла.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Process? Execute(this FileInfo file)
        {
            var processStartInfo = new ProcessStartInfo(file.FullName)
            {
                UseShellExecute = true
            };

            return Process.Start(processStartInfo);
        }
    }
}
