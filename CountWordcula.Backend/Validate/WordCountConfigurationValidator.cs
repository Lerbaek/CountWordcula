using CountWordcula.Backend.Registry;
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

    RuleFor(config => config)
      .Must(BeTrueOrNotOverwriteFiles)
      .WithMessage(config => $"To overwrite existing files, {nameof(config.Force)} flag must be set true");
  }

  public bool BeTrueOrNotOverwriteFiles(WordCountConfiguration config)
  {
    if (config.Force || !BeValidPath(config.OutputPath) || !Directory.Exists(config.OutputPath))
      return true;

    var files = Directory.GetFiles(config.OutputPath);
    if (!files.Any())
      return true;

    for (var c = 'A'; c <= 'Z'; c++)
    {
      var expectedFileName = ConfigurationRegistry.OutputFileName(c);

      if (!files.Select(Path.GetFileName)
            .Contains(expectedFileName))
        continue;

      logger.LogError("Found existing output file: {FileName}", expectedFileName);
      return false;
    }

    return true;
  }

  public bool BeValidExtension(string? extension)
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

  private Func<string?, bool> BeValidPath =>
    path =>
    {
      if (string.IsNullOrWhiteSpace(path))
      {
        logger.LogError("Path may not be empty.");
        return false;
      }
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