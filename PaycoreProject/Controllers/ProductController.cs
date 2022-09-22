using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaycoreProject.Model;
using PaycoreProject.Services.Abstract;

namespace PaycoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;
        private readonly IMapper mapper;
        public ProductController(IProductService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public virtual IActionResult GetAll()
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
        [Authorize]
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
        [Authorize]
        [HttpPost]
        public virtual IActionResult Create([FromBody] ProductDto dto)
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
        [Authorize]
        [HttpPut("{id}")]
        public virtual IActionResult Update(int id, [FromBody] ProductDto dto)
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
        [Authorize]
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
        [HttpPost("buy")]
        public virtual IActionResult Buy(SoldDto sold)
        {
            var result=service.Buy(sold);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest("Product don't buy");
        }
    }
}
