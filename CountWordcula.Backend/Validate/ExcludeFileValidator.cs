using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Backend.Validate;

public class ExcludeFileValidator : LoggingValidatorBase<string[]>
{
  public ExcludeFileValidator(ILogger<ExcludeFileValidator> logger) : base(logger)
  {
    RuleForEach(words => words)
      .Must(BeSingleWord)
      .WithMessage(
        _ =>
          "All excluded words in exclude.txt must be on separate lines with no special characters except \" - \" (dash) or \" ' \" (apostrophe).");
  }

  public bool BeSingleWord(string word)
  {
    if(string.IsNullOrEmpty(word))
    {
      Logger.LogError("Excluded word line may not be empty.");
      return false;
    }

    var invalidCharacters = word.Where(
        c => c is not (>= '0' and <= '9')
          and not (>= 'A' and <= 'Z')
          and not (>= 'a' and <= 'z')
          and not '-'
          and not '\'')
      .ToArray();

    if (!invalidCharacters.Any())
      return true;

    Logger.LogError("Word {Word} contains the following invalid character(s): '{InvalidCharacters}'", word, string.Join("', '", invalidCharacters));
    return false;
  }
}