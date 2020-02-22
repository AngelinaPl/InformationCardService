using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
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

        #region LoadCardAsyncCommand

        private RelayCommand _loadCardsAsyncCommand;
        public ICommand LoadCardsAsyncCommand => _loadCardsAsyncCommand ?? (_loadCardsAsyncCommand = new RelayCommand(LoadCardsAsync));

        public async void LoadCardsAsync()
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
            _currentDialog = new InformationCardDialog();
            _currentDialog.DataContext = this;
            _currentDialog.ShowDialog();
            if (!_isSaved)
            {
                SelectedCard = beforeSavedCard;
            }
            _isSaved = false;
        }

        #endregion

        #region SortByNameCommand

        private RelayCommand _sortByNameCommand;

        public RelayCommand SortByNameCommand =>
            _sortByNameCommand ?? (_sortByNameCommand = new RelayCommand(SortByName));

        public void SortByName()
        
        {
            if (InformationCards != null && InformationCards.Count != 0)
            {
                var ordered =  InformationCards.OrderBy(x => x.Name).ToList();
                InformationCards = new ObservableCollection<InformationCard>(ordered);
            }
        }

        #endregion

        #region ShowCreateCardCommand

        private RelayCommand _showCreateCardCommand;

        public RelayCommand ShowCreateCardCommand =>
            _showCreateCardCommand ?? (_showCreateCardCommand = new RelayCommand(ShowCreateCard));

        public void ShowCreateCard()
        {
            SelectedCard = new InformationCard(0, "", null);
            _currentDialog = new InformationCardDialog();
            _currentDialog.DataContext = this;
            _currentDialog.ShowDialog();
            if (!_isSaved)
            {
                SelectedCard = null;
            }
            _isSaved = false;
        }

        #endregion

        #region DeleteCardAsyncCommand

        private RelayCommand _deleteCardAsyncCommand;

        public RelayCommand DeleteCardAsyncCommand =>
            _deleteCardAsyncCommand ?? (_deleteCardAsyncCommand = new RelayCommand(DeleteCardAsync));

        public async void DeleteCardAsync()
        {
            await _cardService.DeleteCardAsync(SelectedCard.Id);
            InformationCards.Remove(SelectedCard);
        }

        #endregion

        #region SaveCardAsyncCommand

        private RelayCommand _saveCardAsyncCommand;
        public RelayCommand SaveCardAsyncCommand => _saveCardAsyncCommand ?? (_saveCardAsyncCommand = new RelayCommand(SaveCardAsync));

        public async void SaveCardAsync()
        {
            if (SelectedCard.Image != null && !string.IsNullOrEmpty(SelectedCard.Name))
            {
                _isSaved = true;
                await _cardService.SaveCardAsync(SelectedCard);
            }
            _currentDialog?.Close();
            _currentDialog = null;
            LoadCardsAsync();
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