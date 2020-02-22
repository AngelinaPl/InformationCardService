using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using InformationCardService.Client.Annotations;
using InformationCardService.Client.Services;
using InformationCardService.Client.Views;
using InformationCardService.Common;
using Microsoft.Win32;

namespace InformationCardService.Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CardService _cardService = new CardService();
        private Window _currentDialog;
        private ObservableCollection<InformationCard> _informationCards;

        private bool _isSaved;

        private InformationCard _selectedCard;

        public InformationCard SelectedCard
        {
            get => _selectedCard;
            set
            {
                _selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }


        public ObservableCollection<InformationCard> InformationCards
        {
            get => _informationCards;
            set
            {
                _informationCards = value;
                OnPropertyChanged(nameof(InformationCards));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region LoadCardCommand

        private RelayCommand _loadCardsCommand;
        public ICommand LoadCardsCommand => _loadCardsCommand ?? (_loadCardsCommand = new RelayCommand(LoadCards));

        public async void LoadCards()
        {
            var result = await _cardService.GetCardsAsync();
            InformationCards = result?.Object;
        }

        #endregion

        #region ShowUpdateCardCommand

        private RelayCommand _showUpdateCardCommand;

        public RelayCommand ShowUpdateCardCommand =>
            _showUpdateCardCommand ?? (_showUpdateCardCommand = new RelayCommand(ShowUpdateCard));

        public void ShowUpdateCard()
        {
            var beforeSavedCard = SelectedCard;
            _currentDialog = new InformationCardUpdate();
            _currentDialog.DataContext = this;
            _currentDialog.ShowDialog();
            if (!_isSaved)
            {
                SelectedCard = beforeSavedCard;
            }
            _isSaved = false;
        }

        #endregion

        #region SaveCardCommand

        private RelayCommand _saveCardCommand;
        public RelayCommand SaveCardCommand => _saveCardCommand ?? (_saveCardCommand = new RelayCommand(SaveCard));

        public async void SaveCard()
        {
            _isSaved = true;
            await _cardService.SaveCardAsync(SelectedCard);
            _currentDialog?.Close();
            _currentDialog = null;
        }

        #endregion

        #region ChooseImageCommand

        private RelayCommand _chooseImageCommand;

        public RelayCommand ChooseImageCommand =>
            _chooseImageCommand ?? (_chooseImageCommand = new RelayCommand(ChooseImage));

        public void ChooseImage()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
                };
                if (dialog.ShowDialog() ?? false)
                {
                    var file = dialog.FileName;
                    SelectedCard.Image = File.ReadAllBytes(file);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion
    }
}