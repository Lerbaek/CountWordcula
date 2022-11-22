using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Backend.Validate;

public class ExcludeFileValidator : AbstractValidator<string[]>
{
  private readonly ILogger<ExcludeFileValidator> logger;

  public ExcludeFileValidator(ILogger<ExcludeFileValidator> logger)
  {
    this.logger = logger;

    RuleForEach(words => words)
      .Must(BeSingleWord)
      .WithMessage(
        _ =>
          "All excluded words in exclude.txt must be on separate lines with no special characters except \" - \" (dash) or \" ' \" (apostrophe).");
  }

  public bool BeSingleWord(string word)
  {
    var invalidCharacters = word.Where(
        c => c is not (>= '0' and <= '9')
          and not (>= 'A' and <= 'Z')
          and not (>= 'a' and <= 'z')
          and not '-'
          and not '\'')
      .ToArray();

    if (!invalidCharacters.Any())
      return true;

    logger.LogError("Word {Word} contains the following invalid character(s): '{InvalidCharacters}'", word, string.Join("', '", invalidCharacters));
    return false;
  }
}