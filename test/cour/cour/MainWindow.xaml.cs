using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cour
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        HuffmanCode _huffman;
        CRC cycle;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBoxtexChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxtexChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxtexChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxtexChanged_3(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBoxtexChanged_4(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            cycle = new CRC(InputMessageCRC.Text, Polynom.Text);
            BinaryValues.Text = cycle.BinaryText + "\n" + cycle.GeneratingPolynom;
            //BinaryValues.Text = _crc.GeneratingPolynom;
            CRCResult.Text = cycle.Compute();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            /*
               string input = InputHuffman.Text;
               List<string> out_ = new List<string> { };
               foreach (char t in input)
               {
                   if (!out_.Contains(Convert.ToString(t)))
                   {
                       out_.Add(Convert.ToString(t));
                   }
               }
               string output = null;
               string output1 = null;
               string a= _huffman.Encode();
               var huff = _huffman.Frequencies.Values.ToArray();
               double entropy = 0, length = 0;
               for (int i = 0; i < out_.Count; i++)
               {
                   output += "'" + out_[i] + "'" + "     " + (double)huff[i] / (double)input.Length + "      ";

                   entropy += ((double)huff[i] / (double)input.Length) * Math.Log(((double)huff[i] / (double)input.Length), 2);

                   a = _huffman.Encode(out_[i]);
                   output1 = output;
                   foreach (int bit in a)
                   {
                       if (bit == 1)
                       {
                           output += bit;
                       }
                       else 
                       {
                           output += bit;
                       }


                   }
                   length += ((double)huff[i] / (double)input.Length) * (output.Length - output1.Length);
                   output += Environment.NewLine;
               }
               output += "Result:  ";
               a = _huffman.Encode(input);
               foreach (int bit in a)
               {
                   if (bit==1)
                   {
                       output += bit + "";
                   }
                   else 
                   {
                       output += bit + "";
                   }

               }
               output += Environment.NewLine;
               output += "Entropy is   " + (-entropy);

               output += Environment.NewLine;
               output += "AVG length is    " + length;
               Table.Text = output;
               */
        }

        private void FillTable(Dictionary<char, int> frequencies, Dictionary<char, string> codeTable)
        {
   

            var zip = frequencies.Zip(codeTable,
                                      (freq, code) => new
                                      {
                                          Symbol = freq.Key,
                                          Frequency = freq.Value,
                                          Code = codeTable[freq.Key]
              
                                     });
            foreach (var group in zip.OrderByDescending(x => x.Frequency).GroupBy(x => x.Code.Length))
            {
                foreach (var data in group)
                {
                    Table.Text += data.Symbol + "  " + data.Frequency + "  " + data.Code;
                    Table.Text += Environment.NewLine;
                }

            }
        }


        private void Button_Encode(object sender, RoutedEventArgs e)
        {

            _huffman = new HuffmanCode(InputHuffman.Text);
            FillTable(_huffman.Frequencies, _huffman.CodeTable);
            OutputHuffm.Text = _huffman.Encode();

            var InformationText = new InformationText(InputHuffman.Text);
            Entr.Text = "Энтропия: "+Round(InformationText.ShannonEntropy())+ "\nСредняя длина: " + Round(InformationText.AverageCodingMessageLength()); ;
           // LenghtM.Text = "Средняя длина" + Round(InformationText.AverageCodingMessageLength());

        }

        private string Round(double value)
               => Math.Round(value, 2).ToString();

        private void Button_Decode(object sender, RoutedEventArgs e)
        {
            DecodeMessage.Text = _huffman.Decode(OutputHuffm.Text);
           
        }

        private void Button_CRCCompute(object sender, RoutedEventArgs e)
        {
            cycle = new CRC(InputMessageCRC.Text, Polynom.Text);
            BinaryValues.Text = cycle.BinaryText + "\n" + cycle.GeneratingPolynom;
            //BinaryValues.Text = _crc.GeneratingPolynom;
            CRCResult.Text = cycle.Compute();
        }
    }
   
    } 
