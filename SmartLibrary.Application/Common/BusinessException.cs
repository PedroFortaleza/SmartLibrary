namespace SmartLibrary.Application.Common;

public class BusinessException(string message) : Exception(message) { }

public class NotFoundException(string message) : Exception(message) { }
