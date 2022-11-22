using CountWordcula.Backend;
using CountWordcula.Backend.Validate;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace CountWordcula.Test.Validate;

public class ExcludeFileValidatorTests
{
  private readonly ExcludeFileValidator uut;

  public ExcludeFileValidatorTests(ExcludeFileValidator uut) => this.uut = uut;

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  [InlineData(" PrecedingSpace")]
  [InlineData("TrailingSpace ")]
  [InlineData("Two Words")]
  public async Task Validate_AllValuesAreInvalid_AllFailuresAreRegistered(string invalidLine)
  {
    var validation = await uut.ValidateAsync(new[]{invalidLine});
    validation.IsValid.Should().BeFalse("the value is invalid");
  }

  [Fact]
  public async Task Validate_AllValuesAreValid_ValidationSucceeds()
  {
    var validation = await uut.ValidateAsync(new[]{"EXCLUDED", "WORDS"});
    validation.IsValid.Should().BeTrue("all values are valid");
  }
}