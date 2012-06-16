using System;
using ImageService.Contracts;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfClient
{
    public class DataGridModel : INotifyPropertyChanged
    {
        private ObservableCollection<ImageFileData> imagesFileData;

        public DataGridModel()
        {
            imagesFileData = new ObservableCollection<ImageFileData>();
        }

        public ObservableCollection<ImageFileData> ImagesFileData 
        {
            get 
            { 
                return imagesFileData;
            }
            set
            {
                if (imagesFileData != value)
                {
                    imagesFileData = value;
                    NotifyPropertyChanged("ImagesFileData");
                }
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
