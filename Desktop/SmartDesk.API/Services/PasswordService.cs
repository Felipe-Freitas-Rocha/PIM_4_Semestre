namespace SmartDesk.API.Services
{
    public class PasswordService
    {
        // Gera uma senha aleatória simples (ex: "AbC12DeF")
        public string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[Random.Shared.Next(validChars.Length)];
            }
            return new string(password);
        }

        // Cria o Hash da senha para salvar no banco de dados com segurança
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}