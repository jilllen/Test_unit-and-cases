using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cour
{
    public class CRC
    {
        //поле для хранения значений в двоичном виде
        public string BinaryText { get; private set; }
        //создание степени полинома
        public int GeneratingPolynomDegree { get; private set; }
        //поле для хранения значения полинома
        public string GeneratingPolynom { get; private set; }
        //класс для перевода последовательности символов и полинома в строку для расчета crc
        //битовая строка
        string bstring;
        public static class PolyToString
        {        
            //перевод последовательности символов из 16-ричной в 2-ичную СС
            public static string HexToBinary(string sourceMessage)
            {
                string[] hexNums = sourceMessage.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string hexNum = string.Join("", hexNums);
                int intNum = Convert.ToInt32(hexNum, 16);

                return Convert.ToString(intNum, 2);
            }
            //перевод полинома в двоичную СС
            public static string GeneratingPolynomToBinary(string polynom)
            {
                List<int> degrees = FindAllPolynomDegree(polynom);
                var array = new int[degrees.Max() + 1];

                foreach (int degree in degrees)
                    array[degree] = 1;
                Array.Reverse(array);

                return string.Join("", array);
            }
            //поиск всех степеней полинома
            private static List<int> FindAllPolynomDegree(string polynom)
            {
                var list = new List<int>();
                var regex = new Regex(@"\^(\d+)");

                int degree;
                foreach (Match match in regex.Matches(polynom))
                {
                    degree = int.Parse(match.Groups[1].Value);
                    list.Add(degree);
                }
                list.Add(0);
                return list;
            }
        }
        public CRC(string text, string polynom)
        {
            GeneratingPolynom = PolyToString.GeneratingPolynomToBinary(polynom);
            GeneratingPolynomDegree = GeneratingPolynom.Length - 1;
            BinaryText = PolyToString.HexToBinary(text);
            bstring = AddZerosToEnd(BinaryText, GeneratingPolynomDegree);
        }
        //конструктор
        public CRC()
        {
        }

        //проверка на целостность сообщения
        public string CheckMessageIntegrity(string textWithCRC, string polynom)
         {
             bstring = BinaryText = textWithCRC;
             GeneratingPolynom = polynom;
             GeneratingPolynomDegree = GeneratingPolynom.Length - 1;
             return Compute();
         }
       
        //вычисление
        public string Compute()
        {
            while (bstring.Length > GeneratingPolynomDegree)
            {
                BitStringXORgenPoly();
                RemoveExtraZerosFromBitStringStart();
            }

            return bstring;
        }
        //выполнение логического исключения
        private void BitStringXORgenPoly()
        {
            string strToReplace = bstring.Substring(0, GeneratingPolynom.Length);
            var xorResult = new StringBuilder();

            for (int i = 0; i < GeneratingPolynom.Length; i++)
            {
                if (bstring[i] == GeneratingPolynom[i])
                    xorResult.Append('0');
                else
                    xorResult.Append('1');
            }

            bstring = bstring.Replace(strToReplace, xorResult.ToString());
        }
        //удаление лишних нулей из начала битовой строки
        private void RemoveExtraZerosFromBitStringStart()
        {
            while (bstring.StartsWith("0"))
            {
                if (bstring.Length > GeneratingPolynomDegree)
                    bstring = bstring.Remove(0, 1);
                else
                    return;
            }
        }
        //добавление нулей в конец
        private string AddZerosToEnd(string sourceMessage, int count)
        {
            var newBitStr = new StringBuilder(sourceMessage);
            for (int i = 0; i < count; i++)
                newBitStr.Append("0");

            return newBitStr.ToString();
        }
    }
}
