﻿namespace MeetMe.Application.Common.Exceptions;

public class DuplicateException : Exception
{
    public DuplicateException(string message) : base(message) { }
}