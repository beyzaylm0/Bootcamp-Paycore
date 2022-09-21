using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaycoreProject.Model;
using PaycoreProject.Services.Abstract;

namespace PaycoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService service;
        private readonly IMapper mapper;
        public CategoryController(ICategoryService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }
        [HttpGet]
        public  virtual IActionResult GetAll()
        {
            var result = service.GetAll();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            if (result.Response is null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual IActionResult GetById(int id)
        {
            var result = service.GetById(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            if (result.Response is null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost]
        public virtual IActionResult Create([FromBody] CategoryDto dto)
        {
            var result = service.Insert(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            if (result.Response is null)
            {
                return NoContent();
            }

            if (result.Success)
            {
                return StatusCode(201, result);
            }

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public virtual IActionResult Update(int id, [FromBody] CategoryDto dto)
        {
            var result = service.Update(id, dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            if (result.Response is null)
            {
                return NoContent();
            }

            if (result.Success)
            {
                return StatusCode(200, result);
            }

            return BadRequest(result);
        }

        [HttpDelete]
        public virtual IActionResult Delete(int id)
        {
            var result = service.Remove(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
