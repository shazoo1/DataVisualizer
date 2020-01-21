using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataVisualizer.Desktop.ViewModel
{
    public class DataPreviewViewModel : BaseViewModel
    {
        public IContext Context
        {
            get; 
            private set;
        }
        private IDialogService _dialogService;

        public delegate void OnOkButtonClicked();
        public event OnOkButtonClicked OkButtonClicked;

        #region Bindings
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                RaisePropertyChanged("FilePath");
            }
        }

        private bool _isFileOpen = false;
        public bool IsFileOpen
        {
            get => _isFileOpen;
            set
            {
                _isFileOpen = value;
                RaisePropertyChanged("IsFileOpen");
            }
        }

        private ICommand _openFileCommand;
        public ICommand OpenFileCommand
        {
            get => _openFileCommand;
            set => _openFileCommand = value;
        }
        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get => _refreshCommand;
            set => _refreshCommand = value;
        }

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get => _okCommand;
            set => _okCommand = value;
        }

        private string _delimiter;
        public string Delimiter
        {
            get => _delimiter;
            set
            {
                _delimiter = value;
                RaisePropertyChanged("Delimiter");
            }
        }

        private bool _containsHeader;
        public bool ContainsHeader
        {
            get => _containsHeader;
            set
            {
                _containsHeader = value;
                RaisePropertyChanged("ContainsHeader");
            }
        }
        private DataTable _previewData;
        public DataTable PreviewData
        {
            get => _previewData;
            set
            {
                _previewData = value;
                RaisePropertyChanged("PreviewData");
            }
        }

        #endregion

        public DataPreviewViewModel()
        {
            Initialize();
        }
        public DataPreviewViewModel(IDialogService dialogService, IContext context)
        {
            Initialize();
            _dialogService = dialogService;
            Context = context;
            if(Context != null)
                PreviewData = Context.GetFirstLines(100);
            IsFileOpen = Context.IsConnected;
        }

        public DataPreviewViewModel(IDialogService dialogService, IContext context, string connectionString)
        {
            Initialize();
            _dialogService = dialogService;
            Context = context;
            if (Context != null)
            {
                Context.ReadData(Delimiter, ContainsHeader, connectionString);
                if (Context.IsConnected)
                {
                    PreviewData = Context.GetFirstLines(100);
                    FilePath = connectionString;
                }
                IsFileOpen = Context.IsConnected;
            }
        }

        private void Initialize()
        {
            Delimiter = ";";
            ContainsHeader = false;

            OpenFileCommand = new RelayCommand(new Action<object>(OpenFile));
            RefreshCommand = new RelayCommand(new Action<object>(Refresh));
            OkCommand = new RelayCommand(new Action<object>(OkButtonClick));
        }

        public void ReadData(string delimiter, bool header)
        {
            if (Context.IsConnected)
            {
                Context.ReadData(delimiter, header, null);
                PreviewData = Context.GetFirstLines(100);
                
            }
            else
            {
                _dialogService.ShowWarning("Nothing is opened. Please, open the file first.");
            }
            IsFileOpen = Context.IsConnected;
        }

        public void ReadData(string delimiter, bool header, string fileName)
        {
            Context.ReadData(delimiter, header, fileName);
        }

        public void Refresh(object obj)
        {
            ReadData(Delimiter, ContainsHeader);
        }

        public void OpenFile(object obj)
        {
            var fileName = _dialogService.OpenFile();
            if (!string.IsNullOrEmpty(fileName))
            {
                FilePath = fileName;
                Context.ReadData(Delimiter, ContainsHeader, fileName);
                PreviewData = Context.GetFirstLines(100);
            }
        }

        public void OkButtonClick(object obj)
        {
            OkButtonClicked?.Invoke();
        }
    }
}
