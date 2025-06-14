﻿namespace VFi.NetDevPack.Exceptions;

public class ErrorCodeException : Exception
{
    public ErrorCodeException(string errorCode)
        : this(errorCode, new Exception(errorCode))
    {
        ErrorCode = errorCode;
    }

    private ErrorCodeException(string errorCode, Exception innerException)
        : base($"{typeof(ErrorCodeException)} with error code = {errorCode}", innerException)
    {
        ErrorCode = errorCode;
    }

    protected ErrorCodeException(string errorCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    public string ErrorCode { get; }
}
