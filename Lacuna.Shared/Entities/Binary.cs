using System;
using System.Collections.Generic;
using System.Linq;

namespace Lacuna.Shared.Entities
{
    public class Binary
    {
        #region Private methods
        private static uint[] HexStringToUintArray(string hexString)
        {
            var binaryContent = new List<uint>();
            // Make sure the used string will have an even length
            string converted = (hexString.Length % 2 == 0) ? hexString : "0" + hexString;

            // Converts the hexadecimal string to a binary array
            for (int i = 0; i < converted.Length; i += 2)
            {
                var b = Convert.ToByte(hexString.Substring(i, 2), 16);
                binaryContent.Add(b);
            }

            return binaryContent.ToArray();
        }
        #endregion

        private List<uint> _binary;

        #region Constructors
        public Binary()
        {
            _binary = new List<uint>();
        }

        public Binary(string hexString)
        {
            _binary = new List<uint>(HexStringToUintArray(hexString));
        }

        public Binary(List<uint> binary)
        {
            _binary = binary;
        }

        public Binary(List<byte> bytes)
        {
            _binary = bytes.Select(b => (uint)b).ToList();
        }

        public Binary(byte[] bytes)
        {
            _binary = bytes.Select(b => (uint)b).ToList();
        }
        #endregion

        #region Operators
        public static Binary operator ^(Binary b1, Binary b2)
        {
            var result = new List<uint>();
            var bin1 = new List<uint>(b1.ToUintArray());
            var bin2 = new List<uint>(b2.ToUintArray());

            while (bin1.Count != bin2.Count)
            {
                if (bin1.Count < bin2.Count)
                {
                    bin1.Add(0);
                }
                if (bin1.Count > bin2.Count)
                {
                    bin2.Add(0);
                }
            }

            for (int i = 0; i < bin1.Count; i++)
            {
                uint i1 = bin1[i];
                uint i2 = bin2[i];
                result.Add(i1 ^ i2);
            }

            return new Binary(result);
        }
        #endregion

        #region Utils
        public int Match(List<uint> sequence, int startIndex = 0)
        {
            int baseIndex = startIndex, searchIndex = startIndex, matchIndex = 0;

            if (searchIndex < 0)
                return -1;

            if (sequence.Count == 0)
                return -1;

            while (searchIndex < _binary.Count)
            {
                if (matchIndex == 0)
                {
                    searchIndex = _binary.FindIndex(searchIndex, elem => elem == sequence[0]);

                    if(searchIndex<0)
                        return -1;

                    matchIndex++;
                }
                else
                {
                    if(matchIndex==sequence.Count)
                        return searchIndex-matchIndex+1;
                    else if(searchIndex==_binary.Count-1)
                        return -1;

                    if(_binary[searchIndex+1]==sequence[matchIndex])
                    {
                        matchIndex++;
                    }else{
                        matchIndex = 0;
                    }

                    searchIndex++;
                }
            }

            return -1;
        }

        public int Match(Binary sequence, int startIndex = 0)
        {
            return Match(new List<uint>(sequence.ToUintArray()), startIndex);
        }

        public void Add(byte item)
        {
            _binary.Add(item);
        }

        public void Insert(int position, byte value)
        {
            _binary.Insert(position, value);
        }

        public void Insert(int position, uint value)
        {
            _binary.Insert(position, value);
        }

        public void Concatenate(Binary binary)
        {
            _binary = _binary.Concat(binary.ToUintArray()).ToList();
        }

        public int Size()
        {
            return _binary.Count();
        }

        public uint AtIndex(int index)
        {
            return _binary.ElementAt(index);
        }
        #endregion

        #region Conversions
        public static Binary FromHexString(string hexString)
        {
            return new Binary(new List<uint>(HexStringToUintArray(hexString)));
        }

        public static Binary FromString(string content)
        {
            var binaryContent = new List<uint>();

            foreach (var c in content)
                binaryContent.Add(Convert.ToByte(c));

            return new Binary(binaryContent);
        }

        public override string ToString()
        {
            var hex = "";
            foreach (var b in _binary)
            {
                var hexPair = Convert.ToString(b, 16).ToUpper();
                
                if(hexPair.Length<2)
                    hexPair = "0"+hexPair;

                hex += hexPair;
            }
            return hex;
        }

        public uint[] ToUintArray()
        {
            return _binary.ToArray();
        }
        #endregion
    }
}