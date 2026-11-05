using ECommerceApi.Dtos;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
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
       return Ok(ApiResponse<List<ProductResponse>>.Success(product));
    }
    [HttpGet("{id}")]
    public IActionResult? GetProductById(int id)
    {
        var product = _productService.getProductById(id);
        if(product==null)
        return NotFound(ApiResponse<ProductResponse>.Failure($"product with id {id} is not found"));

        return Ok(ApiResponse<ProductResponse>.Success(product));
    }
    [HttpPost]
    public IActionResult AddProduct([FromBody] ProductRequest request)
    {
        _productService.AddProduct(request);
        return Ok(ApiResponse<ProductRequest>.Success(request, "Your product is added"));
    }
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id,[FromBody] ProductRequest request)
    {
        bool updated = _productService.UpdateProduct(id,request);
        if(updated)
        return Ok(ApiResponse<ProductRequest>.Success(request,"Your product is updated successfully"));
        return NotFound(ApiResponse<ProductRequest>.Failure("Your product is not found"));
    }

    [HttpGet("paged")]
public IActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
{
    var result = _productService.GetPagedProducts(page, pageSize);
    return Ok(ApiResponse<PagedResult<ProductResponse>>.Success(result));
}
[HttpPost("Bulk")]
[Authorize]
public IActionResult AddBulkProducts([FromBody] List<ProductRequest> requests, [FromQuery] bool transaction = true)
    {
        try
        {
           var count = _productService.BulkProduct(requests,transaction);
           return Ok(ApiResponse<int>.Success(count, "All products added"));
        }
        catch(InvalidOperationException e)
        {
            return BadRequest(ApiResponse<int>.Failure(e.Message));
        }
    }
}

