using System;
using System.Security.Cryptography;
using System.Text;
using APIWorkshop.Data;
using APIWorkshop.Model;
using Microsoft.EntityFrameworkCore;

namespace APIWorkshop.Services
{
    public class AuthService
    {
        private readonly BelleCroissantContext _context;

        public AuthService(BelleCroissantContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string message, User user)> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            
            if (user == null)
                return (false, "Usuário não encontrado", null);
            
            if (!VerifyPasswordHash(password, user.PasswordHash))
                return (false, "Senha incorreta", null);
            
            // Atualiza último login
            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return (true, "Login bem-sucedido", user);
        }

        public async Task<(bool success, string message, User user)> Register(string email, string password, string firstName, string lastName)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return (false, "Email já está em uso", null);
            
            var passwordHash = HashPassword(password);
            
            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.Now
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return (true, "Registro bem-sucedido", user);
        }
        
        public async Task<(bool success, string message, string token)> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            
            if (user == null)
                return (false, "Usuário não encontrado", null);
            
            // Gera token de redefinição
            var token = GenerateResetToken();
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddHours(1); // Token expira em 1 hora
            
            await _context.SaveChangesAsync();
            
            // Normalmente aqui enviaria o email com o token, mas não implementaremos isso neste exemplo
            return (true, "Token de redefinição gerado com sucesso", token);
        }
        
        public async Task<(bool success, string message)> ResetPassword(string email, string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => 
                u.Email == email && 
                u.ResetToken == token && 
                u.ResetTokenExpiry > DateTime.Now);
            
            if (user == null)
                return (false, "Token inválido ou expirado");
            
            // Atualiza a senha
            user.PasswordHash = HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            
            await _context.SaveChangesAsync();
            
            return (true, "Senha redefinida com sucesso");
        }
        
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
        
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return storedHash == hashOfInput;
        }
        
        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
