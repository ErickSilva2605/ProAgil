using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        // Geral
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         Task<bool> SaveChangesAsync();

         // EVENTOS
         Task<Evento[]> GetAllEventosAsync(bool includePalestrantes);
         Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes);
         Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);

         // PALESTRANTES
         Task<Palestrante[]> GetAllPalestrantesByNameAsync(string name, bool includeEventos);
        Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos);
    }
}