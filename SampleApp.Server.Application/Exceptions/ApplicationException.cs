﻿namespace SampleApp.Server.Application.Exceptions;
internal class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message)
    {
    }
}