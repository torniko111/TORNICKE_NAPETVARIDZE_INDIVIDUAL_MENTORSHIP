using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetPrimitives
{
    [TestClass]
    public class DataTypesBase
    {
        public bool IsPalindrome(string s)
        {
            Stack<char> polomonic = new Stack<char>();

            s = Regex.Replace(s, "[^a-zA-Z0-9]", String.Empty);
            s = s.ToLower();

            for (int i = 0; i < s.Length; i++)
            {
                if (s.Length % 2 == 1)
                {
                    if (i == s.Length / 2)
                    {
                        continue;
                    }
                }

                if (i < s.Length / 2)
                {
                    polomonic.Push(s[i]);
                }
                else
                {
                    var last = polomonic.Pop();
                    if (last != s[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}