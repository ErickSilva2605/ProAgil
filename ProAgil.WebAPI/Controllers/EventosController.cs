using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IProAgilRepository _repository;
        public EventosController (IProAgilRepository repository) 
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get () 
        {
            try
            {
                var results = await _repository.GetAllEventosAsync(true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }
        }

        [HttpGet ("{EventoId}")]
        public async Task<IActionResult> Get (int eventoId) 
        {
            try
            {
                var result = await _repository.GetEventoByIdAsync(eventoId, true);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }
        }

        [HttpGet ("GetByTema{tema}")]
        public async Task<IActionResult> Get (string tema) 
        {
            try
            {
                var result = await _repository.GetAllEventosByTemaAsync(tema, true);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post (Evento model) 
        {
            try
            {
                _repository.Add(model);

                bool result = await _repository.SaveChangesAsync();

                if(result)
                {
                    return Created($"/api/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put (int eventoId, Evento model) 
        {
            try
            {
                var evento = await _repository.GetEventoByIdAsync(eventoId, false);
                if(evento == null) return NotFound();

                _repository.Update(model);

                bool result = await _repository.SaveChangesAsync();

                if(result)
                {
                    return Created($"/api/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete (int eventoId) 
        {
            try
            {
                var evento = await _repository.GetEventoByIdAsync(eventoId, false);
                if(evento == null) return NotFound();
                
                _repository.Delete(evento);

                bool result = await _repository.SaveChangesAsync();

                if(result)
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }

            return BadRequest();
        }
    }
}