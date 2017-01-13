using System;

namespace EnergyTrading.Data
{
    public static class DataConversionExtensions
    {
        public static ulong FromRowVersion(this byte[] rowversion)
        {
            if (rowversion.Length != 8)
            {
                throw new ArgumentOutOfRangeException(nameof(rowversion), "row versions are expected to be 8 byte arrays");
            }
            var copy = new byte[8];
            Array.Copy(rowversion, copy, 8);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(copy);
            }
            return BitConverter.ToUInt64(copy, 0);
        }
    }
}