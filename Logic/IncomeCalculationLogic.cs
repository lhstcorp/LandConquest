using LandConquestDB.Entities;
using LandConquestDB.Models;
using System;
using System.Collections.Generic;

namespace LandConquest.Logic
{
    public class IncomeCalculationLogic
    {
        private static string playerLvl;
        private enum Manufactures : int
        {
            wood = 0,
            clay = 1,
            coal = 2,
            fur = 3,
            wool = 4,
            soda = 5,
            lime = 6,
            leather = 7
        }
        public static void CountResources(Player _player, PlayerStorage _storage, Taxes _taxes, out Player player, out PlayerStorage storage, out Taxes taxes, out string playerLvlValue)
        {
            playerLvl = "";
            _storage = StorageModel.GetPlayerStorage(_player);
            List<Manufacture> manufactures = ManufactureModel.GetManufactureInfo(_player);
            List<Manufacture> playerLandManufactures = ManufactureModel.GetPlayerLandManufactureInfo(_player);

            for (int i = 0; i < 3; i++)
            {
                WarehouseModel.AddItems(manufactures[i].WarehouseId, ((Manufactures)i).ToString(), Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[i].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[i].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(_taxes.TaxValue) / 5))));
                _player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[i].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[i].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(_taxes.TaxValue) / 5)));
                _player = CheckLvlChange(_player);
            }

            bool manf = true;
            if (playerLandManufactures.Count == 0)
            {
                manf = false;
                playerLandManufactures.Add(new Manufacture());
                playerLandManufactures.Add(new Manufacture());
                playerLandManufactures[0].ManufactureProdStartTime = DateTime.UtcNow;
                playerLandManufactures[0].ManufactureProductsHour = 0;

                playerLandManufactures[1].ManufactureProdStartTime = DateTime.UtcNow;
                playerLandManufactures[1].ManufactureProductsHour = 0;
            }

            string firstResource = "";
            switch (playerLandManufactures[0].ManufactureType)
            {
                case 4:
                    {
                        firstResource = "iron";
                        break;
                    }
                case 5:
                    {
                        firstResource = "gold_ore";
                        break;
                    }
                case 6:
                    {
                        firstResource = "copper";
                        break;
                    }
                case 7:
                    {
                        firstResource = "gems";
                        break;
                    }
                case 8:
                    {
                        firstResource = "leather";
                        break;
                    }
            }

            if (firstResource != "")
            {
                WarehouseModel.AddItems(playerLandManufactures[0].WarehouseId, firstResource, Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(_taxes.TaxValue) / 5))));
                _player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(_taxes.TaxValue) / 5)));
                _player = CheckLvlChange(_player);
            }

            string secondResource = "";
            switch (playerLandManufactures[1].ManufactureType)
            {
                case 4:
                    {
                        secondResource = "iron";
                        break;
                    }
                case 5:
                    {
                        secondResource = "gold_ore";
                        break;
                    }
                case 6:
                    {
                        secondResource = "copper";
                        break;
                    }
                case 7:
                    {
                        secondResource = "gems";
                        break;
                    }
                case 8:
                    {
                        secondResource = "leather";
                        break;
                    }
            }

            if (secondResource != "")
            {
                WarehouseModel.AddItems(playerLandManufactures[1].WarehouseId, secondResource, Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(_taxes.TaxValue) / 5))));
                _player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(_taxes.TaxValue) / 5)));
                _player = CheckLvlChange(_player);
            }

            StorageModel.UpdateStorage(_player, _storage);

            ManufactureModel.UpdateDateTimeForManufacture(manufactures, _player);

            if (manf)
            {
                ManufactureModel.UpdateDateTimeForPlayerLandManufacture(playerLandManufactures, _player);
            }

            player          = _player;
            storage         = _storage;
            taxes           = _taxes;
            playerLvlValue  = playerLvl;
        }

        private static Player CheckLvlChange(Player player)
        {

            while (Math.Pow(player.PlayerLvl, 2) * 500 - player.PlayerExp <= 0)
            {
                player.PlayerLvl += 1;
                playerLvl = player.PlayerLvl.ToString();
            }
            return player;
        }

        public static long CountPlayerExp(Player _player)
        {

            Taxes taxes = new Taxes();
            taxes.PlayerId = _player.PlayerId;
            taxes = TaxesModel.GetTaxesInfo(_player.PlayerId);

            List<Manufacture> manufactures = ManufactureModel.GetManufactureInfo(_player);
            List<Manufacture> playerLandManufactures = ManufactureModel.GetPlayerLandManufactureInfo(_player);

            for (int i = 0; i < 3; i++)
            {
                _player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(manufactures[i].ManufactureProdStartTime).TotalSeconds / 3600) * manufactures[i].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
            }

            string firstResource = "";
            if (playerLandManufactures.Count != 0)
            {
                switch (playerLandManufactures[0].ManufactureType)
                {
                    case 4:
                        {
                            firstResource = "iron";
                            break;
                        }
                    case 5:
                        {
                            firstResource = "gold_ore";
                            break;
                        }
                    case 6:
                        {
                            firstResource = "copper";
                            break;
                        }
                    case 7:
                        {
                            firstResource = "gems";
                            break;
                        }
                    case 8:
                        {
                            firstResource = "leather";
                            break;
                        }
                }

                if (firstResource != "")
                {
                    _player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[0].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[0].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                }

                string secondResource = "";
                switch (playerLandManufactures[1].ManufactureType)
                {
                    case 4:
                        {
                            secondResource = "iron";
                            break;
                        }
                    case 5:
                        {
                            secondResource = "gold_ore";
                            break;
                        }
                    case 6:
                        {
                            secondResource = "copper";
                            break;
                        }
                    case 7:
                        {
                            secondResource = "gems";
                            break;
                        }
                    case 8:
                        {
                            secondResource = "leather";
                            break;
                        }
                }

                if (secondResource != "")
                {
                    _player.PlayerExp += Convert.ToInt32((DateTime.UtcNow.Subtract(playerLandManufactures[1].ManufactureProdStartTime).TotalSeconds / 3600) * playerLandManufactures[1].ManufactureProductsHour * (1 + (1 - Convert.ToDouble(taxes.TaxValue) / 5)));
                }
            }
            return _player.PlayerExp;
        }

    }
}
