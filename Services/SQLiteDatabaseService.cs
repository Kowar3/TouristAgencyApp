using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Services
{
    public class SQLiteDatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public SQLiteDatabaseService(string connectionString)
        {
            _connectionString = connectionString;
            EnsureDatabaseFile();
            EnsureDatabase();
        }
        private void EnsureDatabaseFile()
        {
            var builder = new SqliteConnectionStringBuilder(_connectionString);
            string dbPath = builder.DataSource;

            string? folder = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!File.Exists(dbPath))
            {
                File.Create(dbPath).Close();
            }
        }

        private void EnsureDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Clients (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT,
    LastName TEXT,
    PassportNumber TEXT,
    BirthDate TEXT,
    Email TEXT,
    Phone TEXT
);
CREATE TABLE IF NOT EXISTS TravelPackages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT,
    Price REAL,
    Type TEXT,
    Destination TEXT,
    Details TEXT
);
CREATE TABLE IF NOT EXISTS Reservations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER,
    PackageId INTEGER,
    NumPersons INTEGER,
    ReservationDate TEXT,
    ExtraServices TEXT
);";
            cmd.ExecuteNonQuery();
        }

        public List<Client> GetAllClients()
        {
            var result = new List<Client>();
            using var connection = new SqliteConnection(_connectionString);
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
                    BirthDate = DateTime.Parse(reader.GetString(4)),
                    Email = reader.GetString(5),
                    Phone = reader.GetString(6)
                });
            }
            return result;
        }

        public int AddClient(Client client)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Clients (FirstName, LastName, PassportNumber, BirthDate, Email, Phone)
VALUES ($fn, $ln, $pn, $bd, $em, $ph); SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("$fn", client.FirstName);
            cmd.Parameters.AddWithValue("$ln", client.LastName);
            cmd.Parameters.AddWithValue("$pn", EncryptionService.Encrypt(client.PassportNumber));
            cmd.Parameters.AddWithValue("$bd", client.BirthDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$em", client.Email);
            cmd.Parameters.AddWithValue("$ph", client.Phone);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            return id;
        }
        public void RemoveClient(int clientId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Clients WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", clientId);
            cmd.ExecuteNonQuery();
        }
        public void UpdateClient(Client client)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Clients SET FirstName=$fn, LastName=$ln, PassportNumber=$pn, BirthDate=$bd, Email=$em, Phone=$ph WHERE Id=$id";
            cmd.Parameters.AddWithValue("$fn", client.FirstName);
            cmd.Parameters.AddWithValue("$ln", client.LastName);
            cmd.Parameters.AddWithValue("$pn", EncryptionService.Encrypt(client.PassportNumber));
            cmd.Parameters.AddWithValue("$bd", client.BirthDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$em", client.Email);
            cmd.Parameters.AddWithValue("$ph", client.Phone);
            cmd.Parameters.AddWithValue("$id", client.Id);
            cmd.ExecuteNonQuery();
        }
        public Client? GetClientById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();

            cmd.CommandText = @"SELECT * FROM Clients WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);

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
        public List<TravelPackage> GetAllPackages()
        {
            var result = new List<TravelPackage>();
            using var connection = new SqliteConnection(_connectionString);
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
                string destionation = reader.GetString(4);
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
                pkg.Destination = destionation;
                pkg.Details = pkg.ToString();
                result.Add(pkg);
            }
            return result;
        }
        public Reservation? GetReservationById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Reservations WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);

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
        public int AddPackage(TravelPackage package)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();

            cmd.CommandText = @"INSERT INTO TravelPackages (Name, Price, Type, Destination, Details)
                              VALUES ($n, $p, $t, $des, $d); SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("$n", package.Name);
            cmd.Parameters.AddWithValue("$p", package.Price);
            cmd.Parameters.AddWithValue("$t", package.Type);
            cmd.Parameters.AddWithValue("$des", package.Destination);
               

            if(package is ExcursionPackage) cmd.Parameters.AddWithValue("$d", System.Text.Json.JsonSerializer.Serialize((ExcursionPackage)package));
            if(package is SeaPackage) cmd.Parameters.AddWithValue("$d", System.Text.Json.JsonSerializer.Serialize((SeaPackage)package));
            if(package is MountainPackage) cmd.Parameters.AddWithValue("$d", System.Text.Json.JsonSerializer.Serialize((MountainPackage)package));
            if(package is CruisePackage) cmd.Parameters.AddWithValue("$d", System.Text.Json.JsonSerializer.Serialize((CruisePackage)package));

            int id = Convert.ToInt32(cmd.ExecuteScalar());
            return id;
        }

        public void UpdatePackage(TravelPackage package)
        {

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE TravelPackages SET Name=$n, Price=$p, Type=$t, Destination=$des, Details=$d WHERE Id=$id";
            cmd.Parameters.AddWithValue("$n", package.Name);
            cmd.Parameters.AddWithValue("$p", package.Price);
            cmd.Parameters.AddWithValue("$t", package.Type);
            cmd.Parameters.AddWithValue("$des", package.Destination);
            if (package is ExcursionPackage ex)
                cmd.Parameters.AddWithValue("$d", JsonSerializer.Serialize(ex));
            else if (package is SeaPackage sea)
                cmd.Parameters.AddWithValue("$d", JsonSerializer.Serialize(sea));
            else if (package is MountainPackage mtn)
                cmd.Parameters.AddWithValue("$d", JsonSerializer.Serialize(mtn));
            else if (package is CruisePackage cr)
                cmd.Parameters.AddWithValue("$d", JsonSerializer.Serialize(cr));
            else
                cmd.Parameters.AddWithValue("$d", JsonSerializer.Serialize(package));
            cmd.Parameters.AddWithValue("$id", package.Id);
            cmd.ExecuteNonQuery();
        }

        public void RemovePackage(int packageId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM TravelPackages WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", packageId);
            cmd.ExecuteNonQuery();
        }

        public List<Reservation> GetReservationsByClient(int clientId)
        {
            var result = new List<Reservation>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Reservations WHERE ClientId = $cid";
            cmd.Parameters.AddWithValue("$cid", clientId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Reservation
                {
                    Id = reader.GetInt32(0),
                    ClientId = reader.GetInt32(1),
                    PackageId = reader.GetInt32(2),
                    NumPersons = reader.GetInt32(3),
                    ReservationDate = DateTime.Parse(reader.GetString(4)),
                    ExtraServices = reader.GetString(5)
                });
            }
            return result;
        }

        public int AddReservation(Reservation reservation)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Reservations (ClientId, PackageId, NumPersons, ReservationDate, ExtraServices)
VALUES ($cid, $pid, $num, $date, $extra); SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("$cid", reservation.ClientId);
            cmd.Parameters.AddWithValue("$pid", reservation.PackageId);
            cmd.Parameters.AddWithValue("$num", reservation.NumPersons);
            cmd.Parameters.AddWithValue("$date", reservation.ReservationDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$extra", reservation.ExtraServices);

            int insertedId = Convert.ToInt32(cmd.ExecuteScalar());

            return insertedId;
        }

        public void RemoveReservation(int reservationId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Reservations WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", reservationId);
            cmd.ExecuteNonQuery();
        }

        public void UpdateReservation(int reservationId, int numPersons, string extraInfo)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Reservations set numPersons = $np, ExtraServices = $extra where Id = $Id";
            cmd.Parameters.AddWithValue("$np", numPersons);
            cmd.Parameters.AddWithValue("$extra", extraInfo);
            cmd.Parameters.AddWithValue("$Id", reservationId);
            cmd.ExecuteNonQuery();
        }
    }
}