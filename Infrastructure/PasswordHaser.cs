using System;
using System.Security.Cryptography;
using System.Text;

namespace hyouka_api.Infrastructure
{
    public interface IPasswordHasher
    {
        byte[] Hash(string password, byte[] salt);
    }

    public class PasswordHaser : IPasswordHasher
    {
        private readonly HMACSHA512 x = new HMACSHA512(Encoding.UTF8.GetBytes("hyouka"));
        public byte[] Hash(string password, byte[] salt)
        {
            var b = Encoding.UTF8.GetBytes(password);
            var allByte = new byte[b.Length + salt.Length];
            Buffer.BlockCopy(b, 0, allByte, 0, b.Length);

            Buffer.BlockCopy(salt, 0, allByte, b.Length, salt.Length);

            return x.ComputeHash(allByte);
        }
    }
}