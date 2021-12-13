using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace LandConquestDB.Models
{
    public static class WarModel
    {
        public static void DeclareAWar(string war_id, Land landAttacker, Land landDefender)
        {
            string Query = "INSERT INTO dbo.WarData (war_id, land_attacker_id, land_defender_id, datetime_start) VALUES (@war_id, @land_attacker_id, @land_defender_id, @datetime_start)";

            var Command = new SqlCommand(Query, DbContext.GetSqlConnection());
            // int datetimeResult;
            Command.Parameters.AddWithValue("@war_id", war_id);
            Command.Parameters.AddWithValue("@land_attacker_id", landAttacker.LandId);
            Command.Parameters.AddWithValue("@land_defender_id", landDefender.LandId);
            Command.Parameters.AddWithValue("@datetime_start", DateTime.UtcNow);

            Command.ExecuteNonQuery();

            Command.Dispose();
        }

        public static War GetWarById(War war)
        {
            string query = "SELECT * FROM dbo.WarData WHERE war_id = @war_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@war_id", war.WarId);

            using (var reader = command.ExecuteReader())
            {
                var warId = reader.GetOrdinal("war_id");
                var landAttackerId = reader.GetOrdinal("land_attacker_id");
                var landDefenderId = reader.GetOrdinal("land_defender_id");
                var warDateTimeStart = reader.GetOrdinal("datetime_start");

                while (reader.Read())
                {
                    war.WarId = (reader.GetString(warId));
                    war.LandAttackerId = (reader.GetInt32(landAttackerId));
                    war.LandDefenderId = (reader.GetInt32(landDefenderId));
                    war.DateTimeStart = (reader.GetDateTime(warDateTimeStart));
                }
                reader.Close();
            }

            command.Dispose();
            return war;
        }

        public static int SelectLastIdOfWars()
        {
            string stateQuery = "SELECT * FROM dbo.WarData ORDER BY war_id DESC";
            var stateCommand = new SqlCommand(stateQuery, DbContext.GetSqlConnection());
            string state_max_id = "";
            int count = 0;
            using (var reader = stateCommand.ExecuteReader())
            {
                var stateId = reader.GetOrdinal("war_id");
                while (reader.Read())
                {
                    state_max_id = reader.GetString(stateId);
                    count++;
                }
                reader.Close();
            }

            stateCommand.Dispose();
            return count;
        }

        public static List<War> GetWarsInfo(List<War> wars)
        {
            string query = "SELECT * FROM dbo.WarData";
            List<string> warssWarId = new List<string>();
            List<int> warsLandAttackerId = new List<int>();
            List<int> warsLandDefenderId = new List<int>();
            List<DateTime> warsWarDateTimeStart = new List<DateTime>();

            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            using (var reader = command.ExecuteReader())
            {
                var warId = reader.GetOrdinal("war_id");
                var landAttackerId = reader.GetOrdinal("land_attacker_id");
                var landDefenderId = reader.GetOrdinal("land_defender_id");
                var warDateTimeStart = reader.GetOrdinal("datetime_start");

                while (reader.Read())
                {
                    warssWarId.Add(reader.GetString(warId));
                    warsLandAttackerId.Add(reader.GetInt32(landAttackerId));
                    warsLandDefenderId.Add(reader.GetInt32(landDefenderId));
                    warsWarDateTimeStart.Add(reader.GetDateTime(warDateTimeStart));
                }
                reader.Close();
            }

            command.Dispose();

            for (int i = 0; i < warssWarId.Count; i++)
            {
                wars[i].WarId = warssWarId[i];
                wars[i].LandAttackerId = warsLandAttackerId[i];
                wars[i].LandDefenderId = warsLandDefenderId[i];
                wars[i].DateTimeStart = warsWarDateTimeStart[i];
            }

            warssWarId = null;
            warsLandAttackerId = null;
            warsLandDefenderId = null;
            warsWarDateTimeStart = null;

            return wars;
        }

        public static PlayerArmyInWar GetPlayerArmyInWarInfo(string _playerId, War _war)
        {
            PlayerArmyInWar playerArmyInWar = new PlayerArmyInWar();

            string armyQuery = "SELECT * FROM dbo.PlayerArmyInWarData WHERE player_id = @player_id AND war_id = @war_id";
            var armyCommand = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

            armyCommand.Parameters.AddWithValue("@player_id", _playerId);
            armyCommand.Parameters.AddWithValue("@war_id", _war.WarId);

            using (var reader = armyCommand.ExecuteReader())
            {
                var armyArchersCount = reader.GetOrdinal("army_archers_count");
                var armyInfantryCount = reader.GetOrdinal("army_infantry_count");
                var armyHorsemanCount = reader.GetOrdinal("army_horseman_count");
                var armySiegegunCount = reader.GetOrdinal("army_siegegun_count");
                var armySide = reader.GetOrdinal("army_side");
                var warId = reader.GetOrdinal("war_id");

                while (reader.Read())
                {
                    playerArmyInWar.PlayerId = _playerId;
                    playerArmyInWar.ArmyArchersCount = reader.GetInt32(armyArchersCount);
                    playerArmyInWar.ArmyInfantryCount = reader.GetInt32(armyInfantryCount);
                    playerArmyInWar.ArmyHorsemanCount = reader.GetInt32(armyHorsemanCount);
                    playerArmyInWar.ArmySiegegunCount = reader.GetInt32(armySiegegunCount);
                    playerArmyInWar.ArmySide = reader.GetInt32(armySide);
                    playerArmyInWar.WarId = reader.GetString(warId);
                }
                reader.Close();
            }
            armyCommand.Dispose();

            return playerArmyInWar;
        }

        public static void UpdatePlayerArmyInWarScoreHistory(PlayerArmyInWar _playerArmyInWar, War _war)
        {
            string Query = "UPDATE dbo.PlayerArmyInWarData SET army_archers_count = @army_archers_count, army_infantry_count  = @army_infantry_count," +
                                                                    " army_horseman_count = @army_horseman_count, army_siegegun_count = @army_siegegun_count," +
                                                                    " army_side = @army_side " +
                                                                "WHERE player_id = @player_id AND war_id = @war_id";

            var Command = new SqlCommand(Query, DbContext.GetSqlConnection());

            Command.Parameters.AddWithValue("@army_archers_count", _playerArmyInWar.ArmyArchersCount);
            Command.Parameters.AddWithValue("@army_infantry_count", _playerArmyInWar.ArmyInfantryCount);
            Command.Parameters.AddWithValue("@army_horseman_count", _playerArmyInWar.ArmyHorsemanCount);
            Command.Parameters.AddWithValue("@army_siegegun_count", _playerArmyInWar.ArmySiegegunCount);
            Command.Parameters.AddWithValue("@army_side", _playerArmyInWar.ArmySide);

            Command.Parameters.AddWithValue("@player_id", _playerArmyInWar.PlayerId);
            Command.Parameters.AddWithValue("@war_id", _war.WarId);

            Command.ExecuteNonQuery();

            Command.Dispose();
        }

        public static void InsertPlayerArmyToWarScoreHistory(PlayerArmyInWar _playerArmyInWar, War _war)
        {
            string armyQuery = "INSERT INTO dbo.PlayerArmyInWarData (player_id, army_archers_count, army_infantry_count, army_horseman_count, army_siegegun_count, army_side, war_id) " +
                                                         "VALUES (@player_id, @army_archers_count, @army_infantry_count, @army_horseman_count, @army_siegegun_count, @army_side, @war_id)";
            var armyCommand = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

            armyCommand.Parameters.AddWithValue("@player_id", _playerArmyInWar.PlayerId);
            armyCommand.Parameters.AddWithValue("@army_archers_count", _playerArmyInWar.ArmyArchersCount);
            armyCommand.Parameters.AddWithValue("@army_infantry_count", _playerArmyInWar.ArmyInfantryCount);
            armyCommand.Parameters.AddWithValue("@army_horseman_count", _playerArmyInWar.ArmyHorsemanCount);
            armyCommand.Parameters.AddWithValue("@army_siegegun_count", _playerArmyInWar.ArmySiegegunCount);
            armyCommand.Parameters.AddWithValue("@army_side", _playerArmyInWar.ArmySide);
            armyCommand.Parameters.AddWithValue("@war_id", _war.WarId);

            armyCommand.ExecuteNonQuery();

            armyCommand.Dispose();
        }

        public static void addPlayerArmyToWarScoreHistory(ArmyInBattle _armyInBattle, War _war)
        {
            PlayerArmyInWar playerArmyInWar = GetPlayerArmyInWarInfo(_armyInBattle.PlayerId, _war);

            playerArmyInWar.ArmyArchersCount += _armyInBattle.ArmyArchersCount;
            playerArmyInWar.ArmyInfantryCount += _armyInBattle.ArmyInfantryCount;
            playerArmyInWar.ArmyHorsemanCount += _armyInBattle.ArmyHorsemanCount;
            playerArmyInWar.ArmySiegegunCount += _armyInBattle.ArmySiegegunCount;
            playerArmyInWar.ArmySide = _armyInBattle.ArmySide;
            playerArmyInWar.WarId = _war.WarId;


            if (playerArmyInWar.PlayerId == null)
            {
                playerArmyInWar.PlayerId = _armyInBattle.PlayerId;
                InsertPlayerArmyToWarScoreHistory(playerArmyInWar, _war);
            }
            else
            {
                UpdatePlayerArmyInWarScoreHistory(playerArmyInWar, _war);
            }
        }

        public static ArmyInBattle calculateArmy(List<ArmyInBattle> _armies, int _side)
        {
            ArmyInBattle army = new ArmyInBattle();

            for (int i = 0; i < _armies.Count; i++)
            {
                if (_armies[i].ArmySide == _side)
                {
                    army.ArmyArchersCount += _armies[i].ArmyArchersCount;
                    army.ArmyInfantryCount += _armies[i].ArmyInfantryCount;
                    army.ArmyHorsemanCount += _armies[i].ArmyHorsemanCount;
                    army.ArmySiegegunCount += _armies[i].ArmySiegegunCount;
                    army.ArmySizeCurrent += _armies[i].ArmySizeCurrent;
                }
            }

            return army;

        }

    }
}
