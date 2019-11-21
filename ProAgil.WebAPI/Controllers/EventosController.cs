using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

        [HttpPost ("Upload")]
        public IActionResult Upload () 
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources","Images"); 
                var folderToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                
                if(file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(folderToSave, fileName.Replace("\"", "").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao tentar realizar upload: {ex.Message}");
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

                var idLotes = new List<int>();
                var idRedesSociais = new List<int>();

                model.Lotes.ForEach(item => idLotes.Add(item.Id));
                model.RedeSociais.ForEach(item => idRedesSociais.Add(item.Id));

                var lotes = evento.Lotes.Where(lote => !idLotes.Contains(lote.Id)).ToArray();
                var redesSociais = evento.RedeSociais.Where(redeSocial => !idRedesSociais.Contains(redeSocial.Id)).ToArray();

                if(lotes.Length > 0) _repository.DeleteRange(lotes);
                if(redesSociais.Length > 0) _repository.DeleteRange(redesSociais);

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