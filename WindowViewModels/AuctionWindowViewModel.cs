using LandConquest.DialogWIndows;
using LandConquest.WindowViewModels.Commands;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace LandConquest.WindowViewModels
{
    public class AuctionWindowViewModel : INotifyPropertyChanged
    {
        private Player player;

        public AuctionWindowViewModel()
        {
        }

        private List<AuctionListings> _auctionListings;
        public List<AuctionListings> AuctionListings
        {
            get
            {
                return _auctionListings;
            }
            set
            {
                _auctionListings = value;
                OnPropertyChanged("AuctionListings");
            }
        }


        private AuctionListings _selectedListing;
        public AuctionListings SelectedListing
        {
            get
            {
                return _selectedListing;
            }
            set
            {
                _selectedListing = value;
                OnPropertyChanged("SelectedListing");
            }
        }

        private string _searchName;
        public string SearchName
        {
            get
            {
                return _searchName;
            }
            set
            {
                _searchName = value;
                OnPropertyChanged("SearchName");
            }
        }

        private bool _btnBuyIsEnabled;
        public bool BtnBuyIsEnabled
        {
            get
            {
                return _btnBuyIsEnabled;
            }
            set
            {
                _btnBuyIsEnabled = value;
                OnPropertyChanged("BtnBuyIsEnabled");
            }
        }

        private bool _btnDelIsEnabled;
        public bool BtnDelIsEnabled
        {
            get
            {
                return _btnDelIsEnabled;
            }
            set
            {
                _btnDelIsEnabled = value;
                OnPropertyChanged("BtnDelIsEnabled");
            }
        }

        private object _windowTag;
        public object WindowTag
        {
            get
            {
                return _windowTag;
            }
            set
            {
                _windowTag = value;
                OnPropertyChanged("WindowTag");
            }
        }

        public AuctionWindowViewModel(Player _player)
        {
            player = _player;
        }

        private AuctionWindowCommand _windowLoadedCommand;
        public AuctionWindowCommand WindowLoadedCommand
        {
            get
            {
                return _windowLoadedCommand ??
                       (_windowLoadedCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               WindowLoaded();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }
        private void WindowLoaded()
        {
            AuctionListings = new List<AuctionListings>();
            AuctionListings = AuctionModel.GetListings(AuctionListings);
            BtnBuyIsEnabled = false;
            BtnDelIsEnabled = false;
        }

        private AuctionWindowCommand _createListingCommand;
        public AuctionWindowCommand CreateListingCommand
        {
            get
            {
                return _createListingCommand ??
                       (_createListingCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               CreateListing();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }

        private void CreateListing()
        {
            CreateListingDialog createListingDialog = new CreateListingDialog(player);
            createListingDialog.Owner = Application.Current.MainWindow;
            createListingDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createListingDialog.Show();
        }

        private AuctionWindowCommand _findListingCommand;
        public AuctionWindowCommand FindListingCommand
        {
            get
            {
                return _findListingCommand ??
                       (_findListingCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               FindListing();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }

        private void FindListing()
        {
            WindowLoaded();
            if (!String.IsNullOrWhiteSpace(SearchName))
            {
                AuctionListings = AuctionListings.FindAll(x => x.ItemName.Contains(SearchName));
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Resource name is required!");
            }
        }

        private AuctionWindowCommand _showMyListingsCommand;
        public AuctionWindowCommand ShowMyListingsCommand
        {
            get
            {
                return _showMyListingsCommand ??
                       (_showMyListingsCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               ShowMyListings();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }

        private void ShowMyListings()
        {
            WindowLoaded();
            AuctionListings = AuctionListings.FindAll(x => x.SellerName.Contains(player.PlayerName));
        }

        private AuctionWindowCommand _buyCommand;
        public AuctionWindowCommand BuyCommand
        {
            get
            {
                return _buyCommand ??
                       (_buyCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               Buy();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }

        private void Buy()
        {
            BuyListingDialog inputDialog = new BuyListingDialog();
            int itemAmount;
            if (inputDialog.ShowDialog() == true)
            {
                if (inputDialog.Amount > 0)
                {
                    itemAmount = inputDialog.Amount;
                    Player seller = PlayerModel.GetPlayerById(SelectedListing.SellerId);
                    if (player.PlayerMoney >= SelectedListing.Price * itemAmount)
                    {
                        AuctionModel.BuyListing(itemAmount, player, seller, SelectedListing);
                    }
                    else
                    {
                        WarningDialogWindow.CallWarningDialogNoResult("You haven't got enough money!");
                    }
                }
                else
                {
                    WarningDialogWindow.CallWarningDialogNoResult("Value should be more than 0!");
                }
            }
            WindowLoaded();

        }

        private AuctionWindowCommand _deleteClickCommand;
        public AuctionWindowCommand DeleteClickCommand
        {
            get
            {
                return _deleteClickCommand ??
                       (_deleteClickCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               ButtonDeleteClick();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }

        private void ButtonDeleteClick()
        {
            if (player.PlayerId == SelectedListing.SellerId)
            {
                AuctionModel.DeleteListing(SelectedListing.ListingId);
            }
            WindowLoaded();
        }

        private AuctionWindowCommand _dataGridSelectionChangedCommand;
        public AuctionWindowCommand DataGridSelectionChangedCommand
        {
            get
            {
                return _dataGridSelectionChangedCommand ??
                       (_dataGridSelectionChangedCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               DataGridSelectionChanged();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }

        private void DataGridSelectionChanged()
        {
            if (SelectedListing != null)
            {
                if (player.PlayerId == SelectedListing.SellerId)
                {
                    BtnBuyIsEnabled = false;
                    BtnDelIsEnabled = true;
                }
                else
                {
                    BtnBuyIsEnabled = true;
                    BtnDelIsEnabled = false;
                }
            }
        }

        private AuctionWindowCommand _closeWindowCommand;
        public AuctionWindowCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ??
                       (_closeWindowCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               CloseWindow();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }

        private void CloseWindow()
        {
            Logic.AssistantLogic.CloseWindowByTag(WindowTag = 1);
        }

        private AuctionWindowCommand _updateListingsCommand;
        public AuctionWindowCommand UpdateListingsCommand
        {
            get
            {
                return _updateListingsCommand ??
                       (_updateListingsCommand = new AuctionWindowCommand(obj =>
                       {
                           try
                           {
                               UpdateListings();
                           }
                           catch (Exception ex)
                           {
                               WarningDialogWindow.CallWarningDialogNoResult(ex.Message);
                           }
                       }));
            }
        }

        private void UpdateListings()
        {
            WindowLoaded();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
