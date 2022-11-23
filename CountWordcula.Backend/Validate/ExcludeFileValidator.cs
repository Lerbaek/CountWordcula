using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Backend.Validate;

/// <summary>
/// Validate contents of a file containing words to be excluded on separate lines.
/// </summary>
public class ExcludeFileValidator : LoggingValidatorBase<string[]>
{
  /// <inheritdoc/>
  public ExcludeFileValidator(ILogger<ExcludeFileValidator> logger) : base(logger)
  {
    RuleForEach(words => words)
      .Must(BeSingleWord)
      .WithMessage(
        _ =>
          "All excluded words in exclude.txt must be on separate lines with no special characters except \" - \" (dash) or \" ' \" (apostrophe).");
  }

  /// <summary>
  /// Validate that <paramref name="word"/> is a single word and nothing else.
  /// </summary>
  /// <param name="word">Word to be validated.</param>
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