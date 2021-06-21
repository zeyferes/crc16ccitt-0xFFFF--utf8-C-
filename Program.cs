using System;
using System.Diagnostics;
using System.Text;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "00020101021229300012D156000000000510A93FO3230Q31280012D15600000001030812345678520441115802CN5914BEST TRANSPORT6007BEIJING64200002ZH0104最佳运输0202北京540523.7253031565502016233030412340603***0708A60086670902ME91320016A0112233449988770708123456786304";
            Console.WriteLine(CalculateCRC1(input)); // prints A13A
            Console.WriteLine(CalculateCRC2(input)); // prints A13A
        }

        private static string CalculateCRC1(string input)
        {
            int crc = 0xFFFF;
            int polynomial = 0x1021;
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            foreach(byte b in bytes)
            {
                for (int i = 0; i < 8; i++)
                {
                    bool bit = (b >> (7 - i) & 1) == 1;
                    bool c15 = (crc >> 15 & 1) == 1;
                    crc <<= 1;
                    if (c15 ^ bit)
                    {
                        crc ^= polynomial;
                    }
                }
            }
            crc &= 0xFFFF;
            return crc.ToString("X4").ToUpper();
        }

        private static string CalculateCRC2(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            ushort initialValue = 0xffff;
            ushort poly = 0x1021;
            ushort[] table = new ushort[256];
            ushort temp, a;
            for (int i = 0; i < table.Length; i++)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                    {
                        temp = (ushort)((temp << 1) ^ poly);
                    } else
                    {
                        temp <<= 1;
                    }
                    a <<= 1;
                }
                table[i] = temp;
            }
            for (int i = 0; i < bytes.Length; i++)
            {
                initialValue = (ushort)((initialValue << 8) ^ table[((initialValue >> 8) ^ (0xff & bytes[i]))]);
            }
            return initialValue.ToString("X4");
        }
    }
}
