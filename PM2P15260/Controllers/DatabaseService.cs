using SQLite;
using PM2P15260.Models;

namespace PM2P15260.Controllers
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Sitios>().Wait();
        }

        public async Task<List<Sitios>> GetSitiosAsync()
        {
            return await _database.Table<Sitios>().ToListAsync();
        }

        public async Task<Sitios> GetSitioAsync(int id)
        {
            return await _database.Table<Sitios>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveSitioAsync(Sitios sitio)
        {
            if (sitio.Id != 0)
            {
                return await _database.UpdateAsync(sitio);
            }
            else
            {
                return await _database.InsertAsync(sitio);
            }
        }

        public async Task<int> DeleteSitioAsync(Sitios sitio)
        {
            return await _database.DeleteAsync(sitio);
        }
    }
}
