using System.Text.Json;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Services;

public class ProductService
{
    private readonly AppDbContext _contexts;

    public ProductService(AppDbContext context)
    {
        _contexts = context;
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
         var products =_contexts.Products.ToList();
        return products.Select(MaptoProductResponse).ToList();
    }
    public ProductResponse? getProductById(int Id)
    {
       var product = _contexts.Products.FirstOrDefault(p=> p.ProductId == Id);
       if(product== null)
       return null;

       return MaptoProductResponse(product);
    }

    public void AddProduct(ProductRequest request)
    {
        var newProdut =new Product();
        MaptoProduct(newProdut,request);

        _contexts.Products.Add(newProdut);
        _contexts.SaveChanges();
    }

    public bool UpdateProduct(int id, ProductRequest request)
    {
        var existingProduct =_contexts.Products.FirstOrDefault(u=> u.ProductId == id);
        if (existingProduct== null)
        return false;

        MaptoProduct(existingProduct,request);
        existingProduct.Updatedat = DateTime.Now;
        _contexts.SaveChanges();
        return true;
    }

    public Product? GetProductById(int id)
{
    return _contexts.Products.FirstOrDefault(p => p.ProductId == id);
}
}