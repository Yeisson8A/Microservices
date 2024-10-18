using Microsoft.AspNetCore.Mvc;
using Servicios.Api.Libreria.Core.Entities;
using Servicios.Api.Libreria.Repository;

namespace Servicios.Api.Libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaAutorController : ControllerBase
    {
        private readonly IMongoRepository<AutorEntity> _autorGenericoRepository;

        public LibreriaAutorController(IMongoRepository<AutorEntity> autorGenericoRepository)
        {
            _autorGenericoRepository = autorGenericoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorEntity>>> Get()
        {
            var autores = await _autorGenericoRepository.GetAll();
            return Ok(autores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorEntity>> GetById(string id)
        {
            var autor = await _autorGenericoRepository.GetById(id);
            return Ok(autor);
        }

        [HttpPost]
        public async Task Insert(AutorEntity autor)
        {
            await _autorGenericoRepository.InsertDocument(autor);
        }

        [HttpPut("{id}")]
        public async Task Update(string id, AutorEntity autor)
        {
            autor.Id = id;
            await _autorGenericoRepository.UpdateDocument(autor);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _autorGenericoRepository.DeleteById(id);
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<PaginationEntity<AutorEntity>>> GetPagination(PaginationEntity<AutorEntity> pagination)
        {
            var resultados = await _autorGenericoRepository.GetPagination(pagination);
            return Ok(resultados);
        }
    }
}
