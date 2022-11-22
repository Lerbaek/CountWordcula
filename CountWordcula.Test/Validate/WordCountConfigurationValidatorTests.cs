using CountWordcula.Backend;
using CountWordcula.Backend.Validate;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace CountWordcula.Test.Validate;

public class WordCountConfigurationValidatorTests
{
  private readonly WordCountConfigurationValidator uut;

  public WordCountConfigurationValidatorTests(WordCountConfigurationValidator uut) => this.uut = uut;

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  [InlineData("\n")]
  [InlineData(".a .b")]
  [InlineData("\x0e")]
  public async Task Validate_AllValuesAreInvalid_AllFailuresAreRegistered(string invalidExtension)
  {
    var configuration = new WordCountConfiguration(
      null,
      invalidExtension,
      new string(Path.GetInvalidPathChars()));
    var validation = await uut.ValidateAsync(configuration);
    using (new AssertionScope())
    {
      validation.IsValid.Should().BeFalse("all values are invalid");
      validation.Errors.Should().HaveCount(3);
    }
  }

  [Fact]
  public async Task Validate_AllValuesAreValid_ValidationSucceeds()
  {
    var configuration = new WordCountConfiguration(
      "RelativePath",
      ".a",
      "AnotherPath");
    var validation = await uut.ValidateAsync(configuration);
    validation.IsValid.Should().BeTrue("all values are valid");
  }
}