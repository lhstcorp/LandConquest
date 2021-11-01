using LandConquest.DialogWIndows;
using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LandConquest.Forms
{
    public class AuctionWindowViewModel : INotifyPropertyChanged
    {
        private Player player;

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

        public AuctionWindowViewModel(Player _player)
        {
            player = _player;
        }

        public void AuctionWindowLoaded()
        {
            AuctionListings = new List<AuctionListings>();
            AuctionListings = AuctionModel.GetListings(AuctionListings);
            BtnBuyIsEnabled = false;
            BtnDelIsEnabled = false;
        }

        public void FindListing()
        {
            AuctionWindowLoaded();
            if (!String.IsNullOrWhiteSpace(SearchName))
            {
                AuctionListings = AuctionListings.FindAll(x => x.ItemName.Contains(SearchName));
            }
            else
            {
                WarningDialogWindow.CallWarningDialogNoResult("Resource name is required!");
            }
        }

        public void ShowMyListings()
        {
            AuctionWindowLoaded();
            AuctionListings = AuctionListings.FindAll(x => x.SellerName.Contains(player.PlayerName));
        }

        public void Buy()
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
            AuctionWindowLoaded();

        }

        public void ButtonDeleteClick()
        {
            if (player.PlayerId == SelectedListing.SellerId)
            {
                AuctionModel.DeleteListing(SelectedListing.ListingId);
            }
            AuctionWindowLoaded();
        }

        public void AuctionDataGridSelectionChanged()
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

        public void UpdateListings()
        {
            AuctionWindowLoaded();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
