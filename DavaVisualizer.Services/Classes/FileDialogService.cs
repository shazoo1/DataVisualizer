using DavaVisualizer.Services.Contracts;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavaVisualizer.Services.Classes
{
    public class FileDialogService : IFileDialogService
    {
        private OpenFileDialog _dialog;

        public FileDialogService()
        {
            _dialog = new OpenFileDialog();
        }

        public string OpenFile()
        {
            if (_dialog.ShowDialog() == true)
            {
                return _dialog.FileName;
            }
            else return null;
        }
    }
}
