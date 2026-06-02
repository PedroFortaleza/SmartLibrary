namespace SmartLibrary.Application.Common;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;

    public static ApiResponse<T> Success(T data, string message = "Operação realizada com sucesso")
        => new() { Data = data, Message = message };

    public static ApiResponse<T> Fail(string message)
        => new() { Message = message };
}

public class ApiResponse
{
    public string Message { get; set; } = string.Empty;

    public static ApiResponse Success(string message = "Operação realizada com sucesso")
        => new() { Message = message };

    public static ApiResponse Fail(string message)
        => new() { Message = message };
}
