using Domain.Entidades;
using Domain.Interfaces;
using Microsoft.Data.Sqlite;

namespace Data.Repositorio.SQLite
{
    public class RealizadorRepositorySQLite : IRealizadorRepository
    {
        private readonly string _connectionString;

        public RealizadorRepositorySQLite(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Realizador realizador)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Realizadores (Nome, Nacionalidade) VALUES ($nome, $nac)";
            cmd.Parameters.AddWithValue("$nome", realizador.Nome);
            cmd.Parameters.AddWithValue("$nac",  realizador.Nacionalidade);
            cmd.ExecuteNonQuery();
        }

        public List<Realizador> GetAll()
        {
            var lista = new List<Realizador>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, Nacionalidade FROM Realizadores";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(new Realizador(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            return lista;
        }

        public Realizador? GetById(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, Nacionalidade FROM Realizadores WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? new Realizador(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)) : null;
        }

        public Realizador? GetByNome(string nome)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, Nacionalidade FROM Realizadores WHERE LOWER(Nome) = LOWER($nome)";
            cmd.Parameters.AddWithValue("$nome", nome);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? new Realizador(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)) : null;
        }

        public void Remove(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Realizadores WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
