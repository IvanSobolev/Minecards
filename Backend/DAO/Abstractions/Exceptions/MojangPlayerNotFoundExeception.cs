namespace Backend.DAO.Abstractions.Exceptions;

public class MojangPlayerNotFoundExeception(string username)
    : Exception($"Player with username '{username}' not found.");