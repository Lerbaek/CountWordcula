using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Backend.Validate;

/// <summary>
/// A validation base which, upon validation errors, output the summary to an <see cref="ILogger"/>.
/// </summary>
/// <typeparam name="T">Type to validate.</typeparam>
public abstract class LoggingValidatorBase<T> : AbstractValidator<T>
{
  ///
  protected readonly ILogger Logger;

  /// <inheritdoc cref="LoggingValidatorBase{T}"/>
  protected LoggingValidatorBase(ILogger logger) => Logger = logger;

  /// <inheritdoc/>
  public override ValidationResult Validate(ValidationContext<T> context)
  {
    var validationResult = base.Validate(context);
    return LogValidationErrors(validationResult);
  }

  /// <inheritdoc/>
  public override async Task<ValidationResult> ValidateAsync(
    ValidationContext<T> context,
    CancellationToken cancellation = new())
  {
    var validationResult = await base.ValidateAsync(context, cancellation);
    return LogValidationErrors(validationResult);
  }

  private ValidationResult LogValidationErrors(ValidationResult validationResult)
  {
    if (!validationResult.IsValid)
      foreach (var validationResultError in validationResult.Errors)
        Logger.LogError(validationResultError.ErrorMessage);
    return validationResult;
  }
}