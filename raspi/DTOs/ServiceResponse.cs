namespace raspi.DTOs;

public class ServiceResponse<T>
{
    public T? Data { get; private set; }
    public bool Success { get; private set; }
    public string Message { get; private set; } = "";
    public string ErrorMessage { get; private set; } = "";
    public bool IsException { get; set; }
    public bool IsNotFound { get; private set; }

    public static ServiceResponse<T> SuccessResult(T result)
    {
        return new ServiceResponse<T>
        {
            Data = result,
            Success = true
        };
    }

    public static ServiceResponse<T> FailureResult(string message, T result = default)
    {
        return new ServiceResponse<T>
        {
            Data = result,
            Success = false,
            ErrorMessage = message
        };
    }

    public static ServiceResponse<T> NotFoundResult(string message)
    {
        return new ServiceResponse<T>
        {
            Success = false,
            ErrorMessage = message,
            IsNotFound = true
        };
    }

    public ServiceResponse()
    {
        Success = true;
    }

    public ServiceResponse(T data)
    {
        Data = data;
        Success = true;
    }

    public ServiceResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
        Success = false;
    }
}