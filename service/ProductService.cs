using System.Reflection;
using System.Text.Json;
using Azure;
using ECommerceApi.Caching;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using ECommerceApi.Repostories;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

namespace ECommerceApi.Services;

public class ProductService
{
    private readonly IRepostories<Product> _repostories;
    private readonly AppDbContext _context;
    private readonly ProductCaching _cache;

    public ProductService(IRepostories<Product> repostories, AppDbContext appDbContext, ProductCaching caching)
    {
        _repostories = repostories;
        _context = appDbContext;
        _cache = caching;
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
        return _cache.getorcreate("products:all", () =>
         _repostories.GetAll().Select(MaptoProductResponse).ToList());
    }
  public ProductResponse? getProductById(int id)
{
    return _cache.getorcreate($"products:id:{id}", () =>
    {
        var product = _repostories.GetById(id);
        return product == null ? null : MaptoProductResponse(product);
    });
}
    public void AddProduct(ProductRequest request)
    {
        var newProdut =new Product();
        MaptoProduct(newProdut,request);

        _repostories.Add(newProdut);
        _repostories.SaveChanges();
        _cache.Clear();
        
    }

    public bool UpdateProduct(int id, ProductRequest request)
    {
        var existingProduct =_repostories.GetById(id);
        if (existingProduct== null)
        return false;

        MaptoProduct(existingProduct,request);
        existingProduct.Updatedat = DateTime.Now;
        _repostories.SaveChanges();
        _cache.Clear();
        return true;
    }

    public Product? GetProductById(int id)
{
    return _repostories.GetById(id);
}

   public PagedResult<ProductResponse> GetPagedProducts(int PageNumber, int PageSize)
    {
        if(PageNumber < 1 )  PageNumber = 1;
        if(PageSize < 1) PageSize = 5;
        if(PageSize > 50) PageSize = 50;
        return _cache.getorcreate($"products:paged:{PageNumber}:{PageSize}", () =>{

         var query = _repostories.Query().OrderBy(p => p.ProductId);
         var totalcount = query.Count(); 

         var products = query.Skip((PageNumber -1) * PageSize).Take(PageSize).ToList();
         return new PagedResult<ProductResponse>
         {
             Items = products.Select(MaptoProductResponse).ToList(),
             PageNumber = PageNumber,
             PageSize = PageSize,
             TotalCount = totalcount
         };
        });

    }

    public int BulkProduct(List<ProductRequest> request, bool transaction)
    {
        using var tx = transaction ? _context.Database.BeginTransaction() : null;

        try
        {
            foreach(var r in request)
            {
                if(r.Price <=0)
                throw new InvalidOperationException($"invalid price for {r.Name}");

                var newproduct = new Product();
                MaptoProduct(newproduct,r);
                _context.Products.Add(newproduct);
                _context.SaveChanges();
            }
            tx?.Commit();
            return request.Count; 
        }
        catch
        {
            tx?.Rollback();
            throw;
        }
    }

}