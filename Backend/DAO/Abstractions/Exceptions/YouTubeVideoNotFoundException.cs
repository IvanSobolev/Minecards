namespace Backend.DAO.Abstractions.Exceptions;

public class YouTubeVideoNotFoundException(string videoId)
    : Exception($"A video with the ID '{videoId}' could not be found or does not have a thumbnail.");