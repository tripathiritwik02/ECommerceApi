namespace ECommerceApi.Dtos;

public class ApiResponse<T>
{
    public bool IsSucess {get; set;}
    public string Message {get; set;} = string.Empty;
    public T? Data {get; set;}

    public static ApiResponse<T> Success(T data, string Message = "Success") {
        return new ApiResponse<T>
        {
            IsSucess = true,
            Message = Message,
            Data = data
        };
    }

    public static ApiResponse<T> Failure(string Message)
    {
        return new ApiResponse<T>
        {
            IsSucess = false,
            Message = Message
        };
    }
}