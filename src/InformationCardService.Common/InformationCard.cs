using System.ComponentModel;
using System.Runtime.CompilerServices;
using InformationCardService.Common.Annotations;

namespace InformationCardService.Common
{
    public class InformationCard : INotifyPropertyChanged
    {
        private int _id;

        private byte[] _image;

        private string _name;

        public InformationCard(int id, string name, byte[] image)
        {
            Id = id;
            Name = name;
            Image = image;
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => _name;

            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public byte[] Image
        {
            get => _image;

            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}