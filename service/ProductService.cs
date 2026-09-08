using System.Text.Json;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Services;

public class ProductService
{
    private readonly List<Product> _products;

    public ProductService()
    {
          var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Properties", "Models", "Data", "products.json");
        var jsonData = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _products = JsonSerializer.Deserialize<List<Product>>(jsonData, options) ?? new List<Product>();
    }

    private ProductResponse MaptoProductResponse(Product product)
    {
        var response =new ProductResponse
        {
             Id = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Category = product.Category,
            ImageUrl = product.ImageUrl,
            Active = product.Active
        };
        return response;
    }
    private void MaptoProduct(Product product, ProductRequest request)
    {
          product.Name = request.Name;
        product.Category = request.Category;
        product.Price = request.Price;
        product.Description = request.Description;
        product.StockQuantity = request.Quantity;
    }
    public List<ProductResponse> getAllProduct()
    {
        return _products.Select(MaptoProductResponse).ToList();
    }
    public ProductResponse? getProductById(int Id)
    {
       var product = _products.FirstOrDefault(p=> p.ProductId == Id);
       if(product== null)
       return null;

       return MaptoProductResponse(product);
    }

    public void AddProduct(ProductRequest request)
    {
        var newProdut =new Product();
        MaptoProduct(newProdut,request);
        newProdut.ProductId = _products.Count > 0 ? _products.Max(u => u.ProductId) + 1 : 1;
        _products.Add(newProdut);
    }

    public Boolean UpdateProduct(int id, ProductRequest request)
    {
        var existingProduct =_products.FirstOrDefault(u=> u.ProductId == id);
        if (existingProduct== null)
        return false;

        MaptoProduct(existingProduct,request);
        existingProduct.Updatedat = DateTime.Now;

        return true;
    }

    public Product? GetProductById(int id)
{
    return _products.FirstOrDefault(p => p.ProductId == id);
}
}