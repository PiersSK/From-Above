using System.Text;
using System;
using static DiscSlotContent;

public static class TextEncryption
{
    private static string GetPassKey(DecipherType decipher)
    {
        if (decipher == DecipherType.None) return string.Empty;
        else if (decipher == DecipherType.Alpha) return "ITSSPREADING2024";
        else if (decipher == DecipherType.Beta) return "GARDENOFUMBRA2024";
        else if (decipher == DecipherType.Gamma) return "FROMABOVE2025";
        else return string.Empty;
    }

    public static string EncryptToBase64(string plainText, DecipherType decipher)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] keyBytes = Encoding.UTF8.GetBytes(GetPassKey(decipher));
        byte[] result = new byte[textBytes.Length];

        for (int i = 0; i < textBytes.Length; i++)
        {
            result[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Convert.ToBase64String(result);
    }

    public static string DecryptFromBase64(string encryptedBase64, DecipherType decipher)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);
        byte[] keyBytes = Encoding.UTF8.GetBytes(GetPassKey(decipher));
        byte[] result = new byte[encryptedBytes.Length];

        for (int i = 0; i < encryptedBytes.Length; i++)
        {
            result[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Encoding.UTF8.GetString(result);
    }
}
