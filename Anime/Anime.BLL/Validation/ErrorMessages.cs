namespace Anime.BLL.Validation;
public static class ErrorMessages
{
    public const string AgeRange = "Age must be between {0} and {1}";
    public const string StringLength = "{0} must be between {1} and {2} characters";
    public const string RegexPattern = "{0} must contain only Latin letters and start with capital";
    public const string DateInterval = "Release date must be between {0} and {1}";
    public const string EpisodeCount = "Episode count must be a positive number";
}
