using System.ComponentModel;
﻿using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using InformationCardService.Client.Annotations;
using InformationCardService.Client.Services;
using InformationCardService.Common;

namespace InformationCardService.Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CardService _cardService = new CardService();
        private ObservableCollection<InformationCard> _informationCards;

        private RelayCommand _loadCardsCommand;

        public ObservableCollection<InformationCard> InformationCards
        {
            get => _informationCards;
            set
            {
                _informationCards = value;
                OnPropertyChanged(nameof(InformationCards));
            }
        }

        public ICommand LoadCardsCommand => _loadCardsCommand ?? (_loadCardsCommand = new RelayCommand(LoadCards));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void LoadCards()
        {
            var result = await _cardService.GetCardsAsync();
            InformationCards = result?.Object;
        }
    }
}