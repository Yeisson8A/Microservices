using Microsoft.AspNetCore.Mvc;
using Servicios.Api.Libreria.Core.Entities;
using Servicios.Api.Libreria.Repository;

namespace Servicios.Api.Libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly IMongoRepository<LibroEntity> _libroGenericoRepository;

        public LibroController(IMongoRepository<LibroEntity> libroGenericoRepository)
        {
            _libroGenericoRepository = libroGenericoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroEntity>>> Get()
        {
            var libros = await _libroGenericoRepository.GetAll();
            return Ok(libros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroEntity>> GetById(string id)
        {
            var libro = await _libroGenericoRepository.GetById(id);
            return Ok(libro);
        }

        [HttpPost]
        public async Task Insert(LibroEntity libro)
        {
            await _libroGenericoRepository.InsertDocument(libro);
        }

        [HttpPut("{id}")]
        public async Task Update(string id, LibroEntity libro)
        {
            libro.Id = id;
            await _libroGenericoRepository.UpdateDocument(libro);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _libroGenericoRepository.DeleteById(id);
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<PaginationEntity<LibroEntity>>> GetPagination(PaginationEntity<LibroEntity> pagination)
        {
            var resultados = await _libroGenericoRepository.GetPagination(pagination);
            return Ok(resultados);
        }
    }
}
