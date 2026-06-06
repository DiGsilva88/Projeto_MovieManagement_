using Domain.Entidades;
using Domain.Interfaces;
using Microsoft.Data.Sqlite;

namespace Data.Repositorio.SQLite
{
    public class CategoriaRepositorySQLite : ICategoriaRepository
    {
        private readonly string _connectionString;

        public CategoriaRepositorySQLite(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Categoria categoria)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Categorias (Nome) VALUES ($nome)";
            cmd.Parameters.AddWithValue("$nome", categoria.Nome);
            cmd.ExecuteNonQuery();
        }

        public List<Categoria> GetAll()
        {
            var lista = new List<Categoria>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome FROM Categorias";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(new Categoria(reader.GetInt32(0), reader.GetString(1)));
            return lista;
        }

        public Categoria? GetById(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome FROM Categorias WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? new Categoria(reader.GetInt32(0), reader.GetString(1)) : null;
        }

        public Categoria? GetByNome(string nome)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome FROM Categorias WHERE LOWER(Nome) = LOWER($nome)";
            cmd.Parameters.AddWithValue("$nome", nome);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? new Categoria(reader.GetInt32(0), reader.GetString(1)) : null;
        }

        public void Remove(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Categorias WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
