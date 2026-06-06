using Microsoft.Data.Sqlite;

namespace Data.Repositorio.SQLite
{
    /// <summary>
    /// Cria as tabelas SQLite se não existirem.
    /// Chamado uma vez no arranque da aplicação (Program.cs).
    /// </summary>
    public static class DatabaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Categorias (
                    Id   INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Realizadores (
                    Id           INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome         TEXT NOT NULL,
                    Nacionalidade TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Filmes (
                    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo        TEXT    NOT NULL,
                    Ano           INTEGER NOT NULL,
                    Lingua        TEXT    NOT NULL,
                    Classificacao REAL    NOT NULL,
                    CategoriaId   INTEGER NOT NULL,
                    RealizadorId  INTEGER NOT NULL,
                    FOREIGN KEY (CategoriaId)  REFERENCES Categorias(Id),
                    FOREIGN KEY (RealizadorId) REFERENCES Realizadores(Id)
                );";

            cmd.ExecuteNonQuery();
        }
    }
}
