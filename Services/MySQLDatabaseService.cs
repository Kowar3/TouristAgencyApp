using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Services
{
    public class MySQLDatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public MySQLDatabaseService(string connectionString)
        {
            _connectionString = connectionString;
            EnsureDatabase();
        }

        private void EnsureDatabase()
        {
            var baseConnectionString = _connectionString.Replace("Database=turisticka_agencija;", "");
            using var baseConnection = new MySqlConnection(baseConnectionString);
            baseConnection.Open();
            
            var createDbCmd = baseConnection.CreateCommand();
            createDbCmd.CommandText = "CREATE DATABASE IF NOT EXISTS turisticka_agencija;";
            createDbCmd.ExecuteNonQuery();
            
            var dbConnectionString = baseConnectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Clients (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    PassportNumber TEXT,
    BirthDate DATE,
    Email VARCHAR(100),
    Phone VARCHAR(30)
);
CREATE TABLE IF NOT EXISTS TravelPackages (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(200),
    Price DECIMAL(10,2),
    Type VARCHAR(20),
    Destination VARCHAR(200),
    Details TEXT
);
CREATE TABLE IF NOT EXISTS Reservations (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ClientId INT,
    PackageId INT,
    NumPersons INT,
    ReservationDate DATE,
    ExtraServices TEXT
);";
            cmd.ExecuteNonQuery();
        }

        public List<Client> GetAllClients()
        {
            var result = new List<Client>();
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Clients";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Client
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    PassportNumber = EncryptionService.Decrypt(reader.GetString(3)),
                    BirthDate = reader.GetDateTime(4),
                    Email = reader.GetString(5),
                    Phone = reader.GetString(6)
                });
            }
            return result;
        }

        public int AddClient(Client client)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Clients (FirstName, LastName, PassportNumber, BirthDate, Email, Phone)
VALUES (@fn, @ln, @pn, @bd, @em, @ph); SELECT LAST_INSERT_ID()";
            cmd.Parameters.AddWithValue("@fn", client.FirstName);
            cmd.Parameters.AddWithValue("@ln", client.LastName);
            cmd.Parameters.AddWithValue("@pn", EncryptionService.Encrypt(client.PassportNumber));
            cmd.Parameters.AddWithValue("@bd", client.BirthDate);
            cmd.Parameters.AddWithValue("@em", client.Email);
            cmd.Parameters.AddWithValue("@ph", client.Phone);
            int insertedId = Convert.ToInt32(cmd.ExecuteScalar());
            return insertedId;
        }
        public void RemoveClient(int clientId)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Clients WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", clientId);
            cmd.ExecuteNonQuery();
        }
        public void UpdateClient(Client client)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Clients SET FirstName=@fn, LastName=@ln, PassportNumber=@pn, BirthDate=@bd, Email=@em, Phone=@ph WHERE Id=@id";
            cmd.Parameters.AddWithValue("@fn", client.FirstName);
            cmd.Parameters.AddWithValue("@ln", client.LastName);
            cmd.Parameters.AddWithValue("@pn", EncryptionService.Encrypt(client.PassportNumber));
            cmd.Parameters.AddWithValue("@bd", client.BirthDate);
            cmd.Parameters.AddWithValue("@em", client.Email);
            cmd.Parameters.AddWithValue("@ph", client.Phone);
            cmd.Parameters.AddWithValue("@id", client.Id);
            cmd.ExecuteNonQuery();
        }

        public List<TravelPackage> GetAllPackages()
        {
            var result = new List<TravelPackage>();
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM TravelPackages";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                string type = reader.GetString(3);
                string destination = reader.GetString(4);
                string details = reader.GetString(5);

                TravelPackage pkg = type switch
                {
                    "Sea" => JsonSerializer.Deserialize<SeaPackage>(details) ?? new SeaPackage(),
                    "Mountain" => JsonSerializer.Deserialize<MountainPackage>(details) ?? new MountainPackage(),
                    "Excursion" => JsonSerializer.Deserialize<ExcursionPackage>(details) ?? new ExcursionPackage(),
                    "Cruise" => JsonSerializer.Deserialize<CruisePackage>(details) ?? new CruisePackage(),
                    _ => new SeaPackage(),
                };

                pkg.Id = id;
                pkg.Name = name;
                pkg.Price = price;
                pkg.Type = type;
                pkg.Details = pkg.ToString();

                result.Add(pkg);
            }
            return result;
        }

        public int AddPackage(TravelPackage package)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO TravelPackages (Name, Price, Type, Destination, Details)
VALUES (@n, @p, @t, @des, @d); SELECT LAST_INSERT_ID()";
            cmd.Parameters.AddWithValue("@n", package.Name);
            cmd.Parameters.AddWithValue("@p", package.Price);
            cmd.Parameters.AddWithValue("@t", package.Type);
            cmd.Parameters.AddWithValue("@des", package.Destination);
            if (package is ExcursionPackage) cmd.Parameters.AddWithValue("@d", System.Text.Json.JsonSerializer.Serialize((ExcursionPackage)package));
            if (package is SeaPackage) cmd.Parameters.AddWithValue("@d", System.Text.Json.JsonSerializer.Serialize((SeaPackage)package));
            if (package is MountainPackage) cmd.Parameters.AddWithValue("@d", System.Text.Json.JsonSerializer.Serialize((MountainPackage)package));
            if (package is CruisePackage) cmd.Parameters.AddWithValue("@d", System.Text.Json.JsonSerializer.Serialize((CruisePackage)package));

            int id = Convert.ToInt32(cmd.ExecuteScalar());

            return id;
        }

        public void UpdatePackage(TravelPackage package)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE TravelPackages SET Name=@n, Price=@p, Type=@t, Destination=@des, Details=@d WHERE Id=@id";
            cmd.Parameters.AddWithValue("@n", package.Name);
            cmd.Parameters.AddWithValue("@p", package.Price);
            cmd.Parameters.AddWithValue("@t", package.Type);
            cmd.Parameters.AddWithValue("@des", package.Destination);
            if (package is ExcursionPackage ex)
                cmd.Parameters.AddWithValue("@d", JsonSerializer.Serialize(ex));
            else if (package is SeaPackage sea)
                cmd.Parameters.AddWithValue("@d", JsonSerializer.Serialize(sea));
            else if (package is MountainPackage mtn)
                cmd.Parameters.AddWithValue("@d", JsonSerializer.Serialize(mtn));
            else if (package is CruisePackage cr)
                cmd.Parameters.AddWithValue("@d", JsonSerializer.Serialize(cr));
            else
                cmd.Parameters.AddWithValue("@d", JsonSerializer.Serialize(package));
            cmd.Parameters.AddWithValue("@id", package.Id);
            cmd.ExecuteNonQuery();
        }

        public void RemovePackage(int packageId)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM TravelPackages WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", packageId);
            cmd.ExecuteNonQuery();
        }

        public List<Reservation> GetReservationsByClient(int clientId)
        {
            var result = new List<Reservation>();
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Reservations WHERE ClientId = @cid";
            cmd.Parameters.AddWithValue("@cid", clientId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Reservation
                {
                    Id = reader.GetInt32(0),
                    ClientId = reader.GetInt32(1),
                    PackageId = reader.GetInt32(2),
                    NumPersons = reader.GetInt32(3),
                    ReservationDate = reader.GetDateTime(4),
                    ExtraServices = reader.GetString(5)
                });
            }
            return result;
        }
        public Client? GetClientById(int id)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Clients WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Client
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    PassportNumber = EncryptionService.Decrypt(reader.GetString(3)),
                    BirthDate = reader.GetDateTime(4),
                    Email = reader.GetString(5),
                    Phone = reader.GetString(6)
                };
            }
            return null;
        }
        public Reservation? GetReservationById(int id)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Reservations WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Reservation
                {
                    Id = reader.GetInt32(0),
                    ClientId = reader.GetInt32(1),
                    PackageId = reader.GetInt32(2),
                    NumPersons = reader.GetInt32(3),
                    ReservationDate = reader.GetDateTime(4),
                    ExtraServices = reader.GetString(5)
                };
            }

            return null;
        }
        public int AddReservation(Reservation reservation)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Reservations (ClientId, PackageId, NumPersons, ReservationDate, ExtraServices)
VALUES (@cid, @pid, @num, @date, @extra); SELECT LAST_INSERT_ID()";
            cmd.Parameters.AddWithValue("@cid", reservation.ClientId);
            cmd.Parameters.AddWithValue("@pid", reservation.PackageId);
            cmd.Parameters.AddWithValue("@num", reservation.NumPersons);
            cmd.Parameters.AddWithValue("@date", reservation.ReservationDate);
            cmd.Parameters.AddWithValue("@extra", reservation.ExtraServices);
            int insertedId = Convert.ToInt32(cmd.ExecuteScalar());
            return insertedId;
        }

        public void RemoveReservation(int reservationId)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Reservations WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", reservationId);
            cmd.ExecuteNonQuery();
        }

        public void UpdateReservation(int reservationId, int numPersons, string extraInfo)
        {
            var dbConnectionString = _connectionString + "Database=turisticka_agencija;";
            using var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Reservations set numPersons = @np, ExtraServices = @extra where Id = @Id";
            cmd.Parameters.AddWithValue("@np", numPersons);
            cmd.Parameters.AddWithValue("@extra", extraInfo);
            cmd.Parameters.AddWithValue("@Id", reservationId);
            cmd.ExecuteNonQuery();
        }
    }
}

