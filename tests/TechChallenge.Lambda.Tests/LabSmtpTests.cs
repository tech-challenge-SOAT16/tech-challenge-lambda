using TechChallenge.Lambda;

namespace TechChallenge.Lambda.Tests;

public class LabSmtpTests
{
    private static Settings Cloud(bool optIn, string host) => new()
    {
        DatabaseConnection = "Host=db;Database=auth;Username=auth;Password=test;SSL Mode=VerifyFull",
        PrivateKey = "unused-in-validation",
        OtpSecret = new string('x', 32),
        Environment = "homolog",
        Issuer = "https://auth.example.test",
        KeyId = "lab-key",
        Notifier = "smtp",
        AllowLabSmtp = optIn,
        SmtpHost = host
    };

    [Fact]
    public void CloudSmtpRequiresExplicitOptIn() =>
        Assert.Throws<InvalidOperationException>(() => Cloud(false, "internal-mailpit.example.test").Validate());

    [Fact]
    public void CloudSmtpRejectsDefaultLocalHost() =>
        Assert.Throws<InvalidOperationException>(() => Cloud(true, "localhost").Validate());

    [Fact]
    public void CloudSmtpAcceptsExplicitLabConfiguration() =>
        Cloud(true, "internal-mailpit.example.test").Validate();
}
