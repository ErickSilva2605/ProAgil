using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IProAgilRepository _repository;
        private readonly IMapper _mapper;
        public EventosController (IProAgilRepository repository, IMapper mapper) 
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get () 
        {
            try
            {
                var eventos = await _repository.GetAllEventosAsync(true);

                var results = _mapper.Map<EventoDto[]>(eventos);

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
                var evento = await _repository.GetEventoByIdAsync(eventoId, true);

                var result = _mapper.Map<EventoDto>(evento);

                return Ok(result);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }
        }

        [HttpGet ("GetByTema/{tema}")]
        public async Task<IActionResult> Get (string tema) 
        {
            try
            {
                var eventos = await _repository.GetAllEventosByTemaAsync(tema, true);

                var results = _mapper.Map<EventoDto[]>(eventos);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post (EventoDto model) 
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _repository.Add(evento);

                bool result = await _repository.SaveChangesAsync();

                if(result)
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }

            return BadRequest();
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put (int eventoId, EventoDto model) 
        {
            try
            {
                var evento = await _repository.GetEventoByIdAsync(eventoId, false);
                if(evento == null) return NotFound();

                _mapper.Map(model, evento);

                _repository.Update(evento);

                bool result = await _repository.SaveChangesAsync();

                if(result)
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar informações no banco.");
            }

            return BadRequest();
        }

        [HttpDelete("{EventoId}")]
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