using ECommerceApi.Dtos;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService){
        _productService = productService;
    }
    [HttpGet]
    public IActionResult GetAllProduct()
    {
       var product= _productService.getAllProduct();
       return Ok(product);
    }
    [HttpGet("{id}")]
    public IActionResult? GetProductById(int id)
    {
        var product = _productService.getProductById(id);
        if(product==null)
        return NotFound($"product with id ={id} not exist ");

        return Ok(product);
    }
    [HttpPost]
    public IActionResult AddProduct([FromBody] ProductRequest request)
    {
        _productService.AddProduct(request);
        return Ok("products added");
    }
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id,[FromBody] ProductRequest request)
    {
        bool updated = _productService.UpdateProduct(id,request);
        if(updated)
        return Ok("product updated");
        return NotFound($"product with id {id} not found");
    }

}

