using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Backend.Validate;

public abstract class LoggingValidatorBase<T> : AbstractValidator<T>
{
  protected readonly ILogger Logger;

  protected LoggingValidatorBase(ILogger logger) => Logger = logger;

  public override ValidationResult Validate(ValidationContext<T> context)
  {
    var validationResult = base.Validate(context);
    return LogValidationErrors(validationResult);
  }

  public override async Task<ValidationResult> ValidateAsync(
    ValidationContext<T> context,
    CancellationToken cancellation = new CancellationToken())
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