﻿namespace PassIn.Communication.Responses;
public class ResponseErrorJson(string message)
{
    public string Message { get; set; } = message;
}
