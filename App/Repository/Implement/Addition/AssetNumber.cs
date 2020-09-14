using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TREASURY.Repository.Implement.Addition
{
    public static class AssetNumber
    {
        private static Random random = new Random();
        public static string Generate()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
