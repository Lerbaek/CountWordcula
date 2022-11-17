using CountWordcula.Count;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Validate;

public class CountWordsValidator : AbstractValidator<CountWords>
{
  private readonly ILogger logger = null!;

  public CountWordsValidator(ILogger<CountWordsValidator> logger)
  {
    this.logger = logger;

    RuleFor(cw => cw.InputPath)
      .Must(BeValidPath)
      .WithMessage(cw => $"Invalid {nameof(cw.InputPath)} provided: {cw.InputPath}");

    RuleFor(cw => cw.OutputPath)
      .Must(BeValidPath)
      .WithMessage(cw => $"Invalid {nameof(cw.OutputPath)} provided: {cw.OutputPath}");

    RuleFor(cw => cw.Extension)
      .Must(BeValidExtension)
      .WithMessage(cw => $"Invalid {nameof(cw.Extension)} provided: {cw.Extension}");
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