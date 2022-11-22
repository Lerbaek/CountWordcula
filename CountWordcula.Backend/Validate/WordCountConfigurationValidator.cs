using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Backend.Validate;

public class WordCountConfigurationValidator : AbstractValidator<WordCountConfiguration>
{
  private readonly ILogger<WordCountConfigurationValidator> logger;

  public WordCountConfigurationValidator(ILogger<WordCountConfigurationValidator> logger)
  {
    this.logger = logger;

    RuleFor(config => config.InputPath)
      .Must(BeValidPath)
      .WithMessage(config => $"Invalid {nameof(config.InputPath)} provided: {config.InputPath}");

    RuleFor(config => config.OutputPath)
      .Must(BeValidPath)
      .WithMessage(config => $"Invalid {nameof(config.OutputPath)} provided: {config.OutputPath}");

    RuleFor(config => config.InputExtension)
      .Must(BeValidExtension)
      .WithMessage(config => $"Invalid {nameof(config.InputExtension)} provided: {config.InputExtension}");
  }

  public bool BeValidExtension(string extension)
  {
    if (string.IsNullOrWhiteSpace(extension))
    {
      logger.LogError("File extension may not be empty.");
      return false;
    }

    if (extension.Any(e => e.Equals(' ')))
    {
      logger.LogError("File extension may not contain spaces.");
      return false;
    }

    var invalidCharacters = extension.Intersect(Path.GetInvalidPathChars()).ToArray();
    if (invalidCharacters.Any())
    {
      logger.LogError(
        "File contains the following illegal characters: {InvalidCharacters}",
        new string(invalidCharacters));
      return false;
    }

    return true;
  }

  private Func<string, bool> BeValidPath =>
    path =>
    {
      try
      {
        Path.GetFullPath(path);
        return true;
      }
      catch (Exception e)
      {
        logger.LogError(
          e,
          "{InvalidPath} is not a valid path.",
          path);
        return false;
      }
    };
}