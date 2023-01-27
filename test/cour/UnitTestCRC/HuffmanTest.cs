using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using cour;

namespace UnitTestCRC
{
    [TestClass]
    public class HuffmanTest
    {
        [TestMethod]
        public void EncondeAndDecode_WithGivenFrequenciesTest()
        {
            string message = "rfgggghh";
            var alphabetFreq = new Dictionary<char, int>()
            {
                ['r'] = 1,
                ['f'] = 1,
                ['g'] = 4,
                ['h'] = 2
            };

            var huffmanTree = new HuffmanCode(alphabetFreq);

            string encode = huffmanTree.Encode(message);
            string decode = huffmanTree.Decode(encode);

            Assert.AreEqual("11011100001010", encode);
            Assert.AreEqual(message, decode);
        }

        [TestMethod]
        public void EncondeAndDecode_WithTextTest()
        {
            string text = "rfgggghh";
            var huffmanTree = new HuffmanCode(text);

            string encode = huffmanTree.Encode(text);
            string decode = huffmanTree.Decode(encode);

            Assert.AreEqual("11011100001010", encode);
            Assert.AreEqual(text, decode);
        }

        [TestMethod]
        public void Coursework_Huffman_Test()
        {
            string text = "that programmed within each radio unit. When it";
            var a = text.Length;
            var huffmanTree = new HuffmanCode(text);

            var b = huffmanTree.CodeTable.OrderBy(x => x.Value.Length).ToList();
            string encode = huffmanTree.Encode(text);
            string decode = huffmanTree.Decode(encode);

            Assert.AreEqual(text, decode);
        }
    }
}
