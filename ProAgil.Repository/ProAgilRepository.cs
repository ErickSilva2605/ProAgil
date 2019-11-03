using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository 
{
    public class ProAgilRepository : IProAgilRepository 
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
        }

        // GERAL
        public void Add<T> (T entity) where T : class 
        {
            _context.Add(entity);
        }
        public void Update<T> (T entity) where T : class 
        {
            _context.Update(entity);
        }

        public void Delete<T> (T entity) where T : class 
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync () 
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        // EVENTOS
        public async Task<Evento> GetEventoByIdAsync (int eventoId, bool includePalestrantes = false) 
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(i => i.Lotes)
                .Include(i => i.RedeSociais);
            if(includePalestrantes)
            {
                query =  query
                    .Include(i => i.PalestrantesEventos)
                    .ThenInclude(i => i.Palestrante);
            }

            query = query.Where(w => w.Id == eventoId);
            
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Evento[]> GetAllEventosAsync (bool includePalestrantes = false) 
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(i => i.Lotes)
                .Include(i => i.RedeSociais);
            if(includePalestrantes)
            {
                query =  query
                    .Include(i => i.PalestrantesEventos)
                    .ThenInclude(i => i.Palestrante);
            }

            query = query.OrderByDescending(o => o.Data);
            
            return await query.ToArrayAsync();
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync (string tema, bool includePalestrantes) 
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(i => i.Lotes)
                .Include(i => i.RedeSociais);
            if(includePalestrantes)
            {
                query =  query
                    .Include(i => i.PalestrantesEventos)
                    .ThenInclude(i => i.Palestrante);
            }

            query = query
                .OrderByDescending(o => o.Data)
                .Where(w => w.Tema.ToLower().Contains(tema.ToLower()));
            
            return await query.ToArrayAsync();
        }

        // PALESTRANTES
        public async Task<Palestrante> GetPalestranteAsync (int palestranteId, bool includeEventos = false) 
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(i => i.RedeSociais);
            if(includeEventos)
            {
                query =  query
                    .Include(i => i.PalestrantesEventos)
                    .ThenInclude(i => i.Evento);
            }

            query = query.Where(w => w.Id == palestranteId);
            
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesByNameAsync (string name, bool includeEventos = false) 
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(i => i.RedeSociais);
            if(includeEventos)
            {
                query =  query
                    .Include(i => i.PalestrantesEventos)
                    .ThenInclude(i => i.Evento);
            }

            query = query.Where(w => w.Nome.ToLower().Contains(name.ToLower()));
            
            return await query.ToArrayAsync();
        }
    }
}