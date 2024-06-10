using CleanArchMVC.Application.DTOs;
using CleanArchMVC.Application.Interfaces;
using CleanArchMVC.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchMVC.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
    {
        var produtos = await _productService.GetProducts();

        if (produtos == null)
        {
            return NotFound("Categories not found");
        }
        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public async Task<ActionResult<ProductDTO>> Get(int id)
    {
        var produto = await _productService.GetById(id);

        if (produto == null)
        {
            return NotFound("Product not found");
        }
        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProductDTO productDto)
    {
        if (productDto == null)
            return BadRequest("Invalid Data");

        await _productService.Add(productDto);

        return new CreatedAtRouteResult("GetProduct", new { id = productDto.Id },
            productDto);
    }

    [HttpPut]
    public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDto)
    {
        if (id != productDto.Id)
            return BadRequest("Data invalid");

        if (productDto == null)
            return BadRequest("Data invalid");

        await _productService.Update(productDto);

        return Ok(productDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        var productDto = await _productService.GetById(id);

        if (productDto == null)
        {
            return NotFound("Product not found");
        }

        await _productService.Remove(id);

        return Ok(productDto);
    }
}
