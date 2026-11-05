using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace ECommerceApi.Dtos;

public class PagedResult<T>
{
    public List<T> Items {get; set;} = new List<T>();
    public int PageNumber {get; set;}
    public int PageSize {get; set;}
    public int TotalCount {get; set;}

    public int TotalPage => (int)Math.Ceiling(TotalCount/ (double)PageSize);

}