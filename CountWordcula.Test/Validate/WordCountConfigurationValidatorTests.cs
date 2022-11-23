using CountWordcula.Backend;
using CountWordcula.Backend.Registry;
using CountWordcula.Backend.Validate;
using FluentAssertions;
using Xunit;

namespace CountWordcula.Test.Validate;

public class WordCountConfigurationValidatorTests
{
  private readonly WordCountConfigurationValidator uut;
  private readonly WordCountConfiguration validWordCountConfiguration;

  public WordCountConfigurationValidatorTests(WordCountConfigurationValidator uut)
  {
    this.uut = uut;
    validWordCountConfiguration = new WordCountConfiguration(
      "RelativePath",
      ".a",
      ".");
  }

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

    validation.IsValid.Should().BeFalse("all values are invalid");
    validation.Errors.Should().HaveCount(3);
  }

  [Fact]
  public async Task Validate_FileExists_ValidationFails()
  {
    var existingFilePath = Path.Combine(
      validWordCountConfiguration.OutputPath!,
      ConfigurationRegistry.OutputFileName('A'));
    await File.Create(existingFilePath).DisposeAsync();
    var validation = await uut.ValidateAsync(validWordCountConfiguration);
    validation.IsValid.Should()
      .BeFalse("{Force} is false and an output file already exists", nameof(validWordCountConfiguration.Force));
    File.Delete(existingFilePath);
  }

  [Fact]
  public async Task Validate_AllValuesAreValid_ValidationSucceeds()
  {
    var validation = await uut.ValidateAsync(validWordCountConfiguration);
    validation.IsValid.Should().BeTrue("all values are valid");
  }
}