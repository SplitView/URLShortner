namespace Shortner.Application.Exceptions;

public class DuplicateCustomKeyException(string customKey) : Exception($"Url with key {customKey} already exists.");