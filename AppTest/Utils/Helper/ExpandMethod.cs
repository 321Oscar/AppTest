﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Helper
{
    /// <summary>
    /// byte 转换，根据大小端
    /// </summary>
    public static class BitConverterExt
    {
        public static byte[] GetBytes(short data, int byteOrder = 0)
        {
            return GetBytes(data,byteOrder == 1);
        }
        public static byte[] GetBytes(int data, int byteOrder = 0)
        {
            return GetBytes(data, byteOrder == 1);
        }
        public static byte[] GetBytes(short data, bool bigEndian = true)
        {
            if(bigEndian)//大端
            {
                return BitConverter.GetBytes(data);
            }
            else
            {
                data = IPAddress.HostToNetworkOrder(data);
                return BitConverter.GetBytes(data);
            }
        }

        public static byte[] GetBytes(int data, bool bigEndian = true)
        {
            if (bigEndian)//大端
            {
                return BitConverter.GetBytes(data);
            }
            else
            {
                data = IPAddress.HostToNetworkOrder(data);
                return BitConverter.GetBytes(data);
            }
        }

        public static short ToInt16(byte[] data,int startIndex,int byteorder = 0)
        {
            return ToInt16(data, startIndex, byteorder == 0);
        }

        public static short ToInt16(byte[] data, int startIndex, bool bigEndian = true)
        {
            if (bigEndian)//大端
            {
                return BitConverter.ToInt16(new byte[] { data[startIndex + 1], data[startIndex] }, 0);
            }
            else
            {
                return BitConverter.ToInt16(new byte[] { data[startIndex], data[startIndex + 1] }, 0);
            }
        }

        public static ushort ToUInt16(byte[] data, int startIndex, int byteorder = 0)
        {
            return ToUInt16(data, startIndex, byteorder == 0);
        }

        public static ushort ToUInt16(byte[] data, int startIndex, bool bigEndian = true)
        {
            if (bigEndian)//大端
            {
                return BitConverter.ToUInt16(new byte[] { data[startIndex + 1], data[startIndex] }, 0);
            }
            else
            {
                return BitConverter.ToUInt16(new byte[] { data[startIndex], data[startIndex + 1] }, 0);
            }
        }

    }

    public static class ByteExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="m">从右往左数,小的索引</param>
        /// <param name="n">从右往左数,大的索引</param>
        /// <returns></returns>
        public static byte GetBits(this byte b,int m, int n)
        {
            int mask = (1 << (n - m + 1)) - 1;
            byte result = (byte)((b >> m) & mask);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="indx"></param>
        /// <returns></returns>
        public static byte GetBit(this byte b,int indx)
        {
            return (byte)((b >> indx) & 1);
        }
    }
}
