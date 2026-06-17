using System.Security.Cryptography;
using System.Text;

var password = "Admin123*";
var salt = RandomNumberGenerator.GetBytes(16);
var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

var result = string.Join(
    '.',
    "PBKDF2",
    "100000",
    Convert.ToBase64String(salt),
    Convert.ToBase64String(hash));

Console.WriteLine("Hash PBKDF2 para 'Admin123*':");
Console.WriteLine(result);
