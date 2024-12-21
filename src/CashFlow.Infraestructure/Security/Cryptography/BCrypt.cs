﻿using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infraestructure.Security.Cryptography
{
    public class BCrypt : IPasswordEncrypter
    {
        public string Encrypty(string password)
        {
            string passwordHash = BC.HashPassword(password);

            return passwordHash;
        }

        public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
    }
}
