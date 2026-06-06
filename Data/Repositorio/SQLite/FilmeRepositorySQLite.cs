using Domain.Entidades;
using Domain.Interfaces;
using Microsoft.Data.Sqlite;

namespace Data.Repositorio.SQLite
{
    /// <summary>
    /// Persistência em SQLite para Filmes.
    /// Implementa IFilmeRepository — substitui FilmeRepositoryMemory sem tocar em Business ou UI.
    /// </summary>
    public class FilmeRepositorySQLite : IFilmeRepository
    {
        private readonly string _connectionString;

        public FilmeRepositorySQLite(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Filme filme)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Filmes (Titulo, Ano, Lingua, Classificacao, CategoriaId, RealizadorId)
                VALUES ($titulo, $ano, $lingua, $class, $catId, $realId)";

            cmd.Parameters.AddWithValue("$titulo", filme.Titulo);
            cmd.Parameters.AddWithValue("$ano",    filme.Ano);
            cmd.Parameters.AddWithValue("$lingua", filme.Lingua);
            cmd.Parameters.AddWithValue("$class",  filme.Classificacao);
            cmd.Parameters.AddWithValue("$catId",  filme.CategoriaId);
            cmd.Parameters.AddWithValue("$realId", filme.RealizadorId);
            cmd.ExecuteNonQuery();
        }

        public List<Filme> GetAll()
        {
            var lista = new List<Filme>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Titulo, Ano, Lingua, Classificacao, CategoriaId, RealizadorId FROM Filmes";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(MapearFilme(reader));

            return lista;
        }

        public Filme? GetById(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Titulo, Ano, Lingua, Classificacao, CategoriaId, RealizadorId FROM Filmes WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearFilme(reader) : null;
        }

        public Filme? GetByTitulo(string titulo)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Titulo, Ano, Lingua, Classificacao, CategoriaId, RealizadorId FROM Filmes WHERE LOWER(Titulo) = LOWER($titulo)";
            cmd.Parameters.AddWithValue("$titulo", titulo);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapearFilme(reader) : null;
        }

        public void Remove(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Filmes WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private static Filme MapearFilme(SqliteDataReader r) =>
            new Filme(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetString(3), r.GetDouble(4), r.GetInt32(5), r.GetInt32(6));
    }
}
