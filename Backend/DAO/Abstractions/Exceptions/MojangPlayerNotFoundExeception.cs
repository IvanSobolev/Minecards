namespace Backend.DAO.Abstractions.Exceptions;

public class MojangPlayerNotFoundExeception : Exception
{
    public MojangPlayerNotFoundExeception(string username) 
        : base($"Player with username '{username}' not found.") { }
}