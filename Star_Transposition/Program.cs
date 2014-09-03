using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Star_Transposition
{
    class Program
    {
        public static void Initial_OriginalData(string InputFileName, string OutputFileName)
        {
            StreamReader Original = new StreamReader(InputFileName);
            string OriginalData = Original.ReadToEnd();
            Original.Close();
            OriginalData = OriginalData.Replace(" ", "");
            OriginalData = OriginalData.Replace(",", "");
            OriginalData = OriginalData.Replace(".", "");
            OriginalData = OriginalData.ToUpper();
            StreamWriter AfterInitial = new StreamWriter(OutputFileName);
            AfterInitial.Write(OriginalData);
            AfterInitial.Close();
        }
        public static void Initial_HomophonicTable(string OutputFileName)
        {
            int[,] table = new int[26, 10];
            Random rand = new Random();
            int k = 0, table_temp;
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (k > 255)
                        break;
                    table[i, j] = k;
                    k++;
                }
            }
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (i == 25 && j > 5)
                        break;
                    int r1 = rand.Next(0, 25);
                    int r2 = rand.Next(0, 9);
                    while (r1 == 25 && r2 > 5)
                    {
                        r1 = rand.Next(0, 25);
                        r2 = rand.Next(0, 9);
                    }
                    table_temp = table[i, j];
                    table[i, j] = table[r1, r2];
                    table[r1, r2] = table_temp;
                }
            }
            StreamWriter WriteTable = new StreamWriter(OutputFileName);
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    WriteTable.Write("|" + Convert.ToString(table[i, j], 16));
                }
                WriteTable.Write("|");
                WriteTable.WriteLine();
            }
            WriteTable.Close();
        }
        public static void Homophonic_Encryption(string InputFileName, string OutputFileName, string HomophonicTableName)
        {
            Dictionary<string, char> Search = new Dictionary<string, char>();
            Random rand = new Random();
            StreamReader ReadTable = new StreamReader(HomophonicTableName);
            string CurrentLine = "";
            char Character = 'A';
            while ((CurrentLine = ReadTable.ReadLine()) != null)
            {
                int CurrentBar = CurrentLine.IndexOf("|") + 1;
                while (true)
                {
                    try
                    {
                        string CurrentNum = CurrentLine.Substring(CurrentBar, CurrentLine.IndexOf("|", CurrentBar) - CurrentBar);
                        Search.Add(CurrentNum, Character);
                        CurrentBar = CurrentLine.IndexOf("|", CurrentBar) + 1;
                    }
                    catch
                    {
                        break;
                    }
                }
                Character++;
            }
            StreamReader Original = new StreamReader(InputFileName);
            string OriginalData = Original.ReadToEnd();
            Original.Close();
            StreamWriter AfterHomophonic = new StreamWriter(OutputFileName);
            for (int i = 0; i < OriginalData.Length; i++)
            {
                char CurrentChar = OriginalData[i];
                //Console.WriteLine("CurrentChar: " + CurrentChar);
                int randon = rand.Next(1, 10);
                if (CurrentChar == 'Z')
                {
                    randon = randon % 6 + 1;
                }
                int CurrentCharPosition = 0;
                foreach (KeyValuePair<string, char> kvp in Search)
                {
                    //Console.WriteLine("Currentkvp.Value: " + kvp.Value);
                    if (kvp.Value == CurrentChar)
                    {
                        //Console.WriteLine("in");
                        CurrentCharPosition++;
                        if (CurrentCharPosition == randon)
                        {
                            if (kvp.Key.Length == 1)
                            {
                                StringBuilder builder = new StringBuilder();
                                builder.Append("0");
                                builder.Append(kvp.Key);
                                AfterHomophonic.Write(builder.ToString().ToUpper());
                                break;
                            }
                            else
                            {
                                AfterHomophonic.Write(kvp.Key.ToUpper());
                                break;
                            }
                        }
                    }
                }
            }
            AfterHomophonic.Close();
            ReadTable.Close();
            //Console.WriteLine(OriginalData);
        }
        public static void Homophonic_Decryption(string InputFileName, string OutputFileName, string HomophonicTableName)
        {
            Dictionary<string, char> Search = new Dictionary<string, char>();
            Random rand = new Random();
            StreamReader ReadTable = new StreamReader(HomophonicTableName);
            string CurrentLine = "";
            char Character = 'A';
            while ((CurrentLine = ReadTable.ReadLine()) != null)
            {
                int CurrentBar = CurrentLine.IndexOf("|") + 1;
                while (true)
                {
                    try
                    {
                        string CurrentNum = CurrentLine.Substring(CurrentBar, CurrentLine.IndexOf("|", CurrentBar) - CurrentBar);
                        Search.Add(CurrentNum, Character);
                        CurrentBar = CurrentLine.IndexOf("|", CurrentBar) + 1;
                    }
                    catch
                    {
                        break;
                    }
                }
                Character++;
            }
            StreamReader Original = new StreamReader(InputFileName);
            string OriginalData = Original.ReadToEnd();
            Original.Close();
            StreamWriter AfterHomophonicDecrypt = new StreamWriter(OutputFileName);
            for (int i = 0; i < OriginalData.Length; i = i + 2)
            {
                string CurrentString = OriginalData.Substring(i, 2);
                CurrentString = CurrentString.ToLower();
                //Console.WriteLine("CurrentChar: " + CurrentChar);
                int randon = rand.Next(1, 10);
                if (CurrentString.StartsWith("0"))
                {
                    CurrentString = CurrentString.Substring(1, 1);
                }
                AfterHomophonicDecrypt.Write(Search[CurrentString].ToString());
            }
            AfterHomophonicDecrypt.Close();
            ReadTable.Close();
        }
        public static void Star_Transition_Encryption(string InputFileName, string OutputFileName)
        {
            StreamReader Original = new StreamReader(InputFileName);   //讀入上一階段的密文
            string OriginalData = Original.ReadToEnd();
            Original.Close();
            ArrayList Position1 = new ArrayList();
            ArrayList Position2 = new ArrayList();
            ArrayList Position3 = new ArrayList();    //宣告5個位置
            ArrayList Position4 = new ArrayList();
            ArrayList Position5 = new ArrayList();
            int Current = 0;
            while (true)
            {
                Position1.Add(OriginalData.Substring(Current, 2));  //8bit資料放入第一個array
                Current = Current + 2;
                if (Current >= OriginalData.Length) break;
                Position2.Add(OriginalData.Substring(Current, 2));  //8bit資料放入第二個array
                Current = Current + 2;
                if (Current >= OriginalData.Length) break;
                Position3.Add(OriginalData.Substring(Current, 2));  //8bit資料放入第三個array
                Current = Current + 2;
                if (Current >= OriginalData.Length) break;
                Position4.Add(OriginalData.Substring(Current, 2));  //8bit資料放入第四個array
                Current = Current + 2;
                if (Current >= OriginalData.Length) break;
                Position5.Add(OriginalData.Substring(Current, 2));  //8bit資料放入第五個array
                Current = Current + 2;
                if (Current >= OriginalData.Length) break;
            }
            StreamWriter AfterData = new StreamWriter(OutputFileName);
            PrintValues(Position3, AfterData);
            PrintValues(Position5, AfterData);
            PrintValues(Position1, AfterData);
            PrintValues(Position4, AfterData);
            PrintValues(Position2, AfterData);
            AfterData.Close();
        }
        public static void Star_Transition_Decryption(string InputFileName, string OutputFileName)
        {
            StreamReader Original = new StreamReader(InputFileName);   //讀入上一階段的密文
            string OriginalData = Original.ReadToEnd();
            Original.Close();
            ArrayList Position1 = new ArrayList();
            ArrayList Position2 = new ArrayList();
            ArrayList Position3 = new ArrayList();    //宣告5個位置
            ArrayList Position4 = new ArrayList();
            ArrayList Position5 = new ArrayList();
            int finalloop = OriginalData.Length / 2 % 5;    //要知道最後一loop的元素量
            //Console.WriteLine(OriginalData.Length);
            //Console.WriteLine("finalloop" + finalloop);
            int[] finalflag = new int[5];
            for (int i = 0; i < 5; i++)
            {
                finalflag[i] = 0;
                if (i < finalloop)
                {
                    finalflag[i] = 1;
                }
            }
            int persum = OriginalData.Length / 2 / 5;   //要知道每點的總量
            //Console.WriteLine(persum);
            int Current = 0;
            //Console.WriteLine(finalflag[2]);
            while (Current < 2*(persum + finalflag[2]))
            {
                Position3.Add(OriginalData.Substring(Current, 2)); 
                Current = Current + 2;
            }
            while (Current - 2 * (persum + finalflag[2]) < 2 * (persum + finalflag[4]))
            {
                Position5.Add(OriginalData.Substring(Current, 2));
                Current = Current + 2;
            }
            while (Current - 2 * (2 * persum + finalflag[2] + finalflag[4]) < 2 * (persum + finalflag[0]))
            {
                Position1.Add(OriginalData.Substring(Current, 2));
                Current = Current + 2;
            }
            while (Current - 2 * (3 * persum + finalflag[2] + finalflag[4] + finalflag[0]) < 2 * (persum + finalflag[3]))
            {
                Position4.Add(OriginalData.Substring(Current, 2));
                Current = Current + 2;
            }
            while (Current - 2 * (4 * persum + finalflag[2] + finalflag[4] + finalflag[0] + finalflag[3]) < 2 * (persum + finalflag[1]))
            {
                Position2.Add(OriginalData.Substring(Current, 2));
                Current = Current + 2;
            }
            StreamWriter AfterData = new StreamWriter(OutputFileName);
            //Console.WriteLine(Position3.Count);
            for (int i = 0; i < Position1.Count; i++)
            {
                AfterData.Write(Position1[i]);
                if (Position2.Count > i)
                {
                    AfterData.Write(Position2[i]);
                }
                if (Position3.Count > i)
                {
                    AfterData.Write(Position3[i]);
                }
                if (Position4.Count > i)
                {
                    AfterData.Write(Position4[i]);
                }
                if (Position5.Count > i)
                {
                    AfterData.Write(Position5[i]);
                }
            }
             AfterData.Close();
        }
        public static void Variations_Encryption(string InputFileName, string OutputFileName, string KeyFileName)
        {
            StreamReader Original = new StreamReader(InputFileName);
            string OriginalData = Original.ReadToEnd();
            Original.Close();
            string[] CurrentCode = new string[OriginalData.Length/2];
            string[] NewCode = new string[OriginalData.Length / 2];
            //Console.WriteLine("Before Variations : " + OriginalData);
            for (int i = 0; i < OriginalData.Length; i = i + 2)
            {
                CurrentCode[i/2] = OriginalData.Substring(i, 2);
            }
            StreamReader KeyStream = new StreamReader(KeyFileName);
            string KeyString = KeyStream.ReadToEnd();
            int Key = Convert.ToInt32(KeyString);
            //Console.WriteLine("Original Data : ");
            //foreach (string k in CurrentCode)
            //{
            //    Console.Write(k + " ");
            //}
            //Console.WriteLine();
            for (int i = 0; i < Key; i ++)
            {
                for (int j = 0; j < OriginalData.Length / 2; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j == 0)
                        {
                            NewCode[j] = CurrentCode[j];
                            //Console.Write(NewCode[j] + " ");
                        }
                        else
                        {
                            int decNewCode = Convert.ToInt32(NewCode[j - 1], 16);
                            int decCurrentCode = Convert.ToInt32(CurrentCode[j], 16);
                            NewCode[j] =(decNewCode^ decCurrentCode).ToString("x");
                            //Console.Write(NewCode[j] + " ");
                        }
                    }
                    else
                    {
                        int tempj = OriginalData.Length / 2 - 1 - j;
                        if (j == 0)
                        {
                            NewCode[tempj] = CurrentCode[tempj];
                            //Console.Write(NewCode[tempj] + " ");
                        }
                        else
                        {
                            int decNewCode = Convert.ToInt32(NewCode[tempj + 1], 16);
                            int decCurrentCode = Convert.ToInt32(CurrentCode[tempj], 16);
                            NewCode[tempj] = (decNewCode ^ decCurrentCode).ToString("x");
                            //Console.Write(NewCode[tempj] + " ");
                        }
                    }
                }
                //Console.WriteLine();
                NewCode.CopyTo(CurrentCode, 0);
                //Console.WriteLine("loop " + i + " Data : ");
                //foreach (string k in CurrentCode)
                //{
                //    //Console.Write(k + " ");
                //}
                ////Console.WriteLine();
            }
            StringBuilder binbuilder = new StringBuilder();
            StreamWriter AfterVariations = new StreamWriter(OutputFileName);
            for (int i = 0; i < OriginalData.Length/2; i++)
            {
                //Console.WriteLine(Convert.ToInt32(CurrentCode[i], 16));
                if (CurrentCode[i].Length == 1)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("0000");
                    if (Convert.ToInt32(CurrentCode[i], 16) < 8)
                    {
                        builder.Append("0");
                    }
                    if (Convert.ToInt32(CurrentCode[i], 16) < 4)
                    {
                        builder.Append("0");
                    }
                    if (Convert.ToInt32(CurrentCode[i], 16) < 2)
                    {
                        builder.Append("0");
                    }
                    builder.Append(Convert.ToString(Convert.ToInt32(CurrentCode[i], 16), 2));
                    binbuilder.Append(builder);
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    if (Convert.ToInt32(CurrentCode[i], 16) < 128)
                    {
                        builder.Append("0");
                    }
                    if (Convert.ToInt32(CurrentCode[i], 16) < 64)
                    {
                        builder.Append("0");
                    }
                    if (Convert.ToInt32(CurrentCode[i], 16) < 32)
                    {
                        builder.Append("0");
                    }
                    builder.Append(Convert.ToString(Convert.ToInt32(CurrentCode[i], 16), 2));
                    binbuilder.Append(builder);
                }
            }
            //Console.WriteLine(binbuilder.ToString());
            string BinaryCode = binbuilder.ToString();
            StringBuilder finalVariationsCode = new StringBuilder();
            char character = 'A';
            for (int i = 0; i < BinaryCode.Length; i = i + 5)
            {
                string CurrentBinary = "";
                try
                {
                    CurrentBinary = BinaryCode.Substring(i, 5);
                    //Console.Write(CurrentBinary + " ");
                    int Sum = 0;
                    
                    for (int j = 0; j < CurrentBinary.Length; j++)
                    {
                        //Console.WriteLine((Convert.ToInt32(CurrentBinary[j]) - 48));
                        if (CurrentBinary[j] == '1')
                        {
                            Sum = Sum + Convert.ToInt32(Math.Pow(2, CurrentBinary.Length - j - 1));
                        }
                        //Console.Write(Sum + " ");
                    }
                    //Console.WriteLine();
                    //Console.WriteLine(Sum);
                    if (Sum / 25 == 1)
                    {
                        char tempchar = (char)(character + (Sum%25));
                        finalVariationsCode.Append("Z");
                        finalVariationsCode.Append(tempchar.ToString());
                    }
                    else
                    {
                        char tempchar = (char)(character + (Sum % 25));
                        finalVariationsCode.Append(tempchar.ToString());
                    }
                }
                catch
                {
                    StringBuilder finalBinary = new StringBuilder();
                    finalBinary.Append(BinaryCode.Substring(i, BinaryCode.Length - i));
                    int zerobound = BinaryCode.Length - i;
                    zerobound = 5 - zerobound; 
                    for (int j = 0; j < zerobound; j++)
                    {
                        finalBinary.Append("0");
                    }
                    CurrentBinary = finalBinary.ToString();
                    int Sum = 0;
                    for (int j = 0; j < CurrentBinary.Length; j++)
                    {
                        //Console.WriteLine((Convert.ToInt32(CurrentBinary[j]) - 48));
                        if (CurrentBinary[j] == '1')
                        {
                            Sum = Sum + Convert.ToInt32(Math.Pow(2, CurrentBinary.Length - j - 1));
                        }
                        //Console.WriteLine("Cuurent: " + CurrentBinary[j] + " Sum : " + Sum + " Power : " + (CurrentBinary.Length - j - 1));
                    }
                    //Console.WriteLine();
                    //Console.WriteLine(Sum);
                    if (Sum / 25 == 1)
                    {
                        char tempchar = (char)(character + (Sum % 25));
                        finalVariationsCode.Append("Z");
                        finalVariationsCode.Append(tempchar.ToString());
                    }
                    else
                    {
                        char tempchar = (char)(character + (Sum % 25));
                        finalVariationsCode.Append(tempchar.ToString());
                    }
                    //Console.Write(CurrentBinary);
                }
            }
            //Console.WriteLine();
            //Console.WriteLine("After Variations : " + finalVariationsCode.ToString());
            AfterVariations.Write(finalVariationsCode.ToString());
            AfterVariations.Close();
        }
        public static void Variations_Decryption(string InputFileName, string OutputFileName, string KeyFileName)
        {
            StreamReader Original = new StreamReader(InputFileName);
            string OriginalData = Original.ReadToEnd();
            Original.Close();
            string BinData = "";
            //Console.WriteLine("Before Decrypt Variations : " + OriginalData);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < OriginalData.Length; i++)
            {
                if (OriginalData[i] == 'Z')
                {
                    i = i + 1;
                    char temp = (char)(OriginalData[i] - 'A' + 25);
                    //Console.Write((int)temp + "  ");
                    if ((int)temp < 2)
                    {
                        builder.Append("0000");
                    }
                    else if ((int)temp < 4)
                    {
                        builder.Append("000");
                    }
                    else if ((int)temp < 8)
                    {
                        builder.Append("00");
                    }
                    else if ((int)temp < 16)
                    {
                        builder.Append("0");
                    }
                    builder.Append(Convert.ToString((int)temp, 2));
                    //Console.WriteLine(builder.ToString());
                }
                else
                {
                    char temp = (char)(OriginalData[i] - 'A');
                    //Console.Write((int)temp + "  ");
                    if ((int)temp < 2)
                    {
                        builder.Append("0000");
                    }
                    else if ((int)temp < 4)
                    {
                        builder.Append("000");
                    }
                    else if ((int)temp < 8)
                    {
                        builder.Append("00");
                    }
                    else if ((int)temp < 16)
                    {
                        builder.Append("0");
                    }
                    builder.Append(Convert.ToString((int)temp, 2));
                    //Console.WriteLine(builder.ToString());
                }
            }
            //Console.WriteLine();
            BinData = builder.ToString();
            //Console.WriteLine("Before Decrypt Variations : " + BinData);
            StringBuilder hexbuilder = new StringBuilder();
            for (int i = 0; i < BinData.Length; i = i + 4)
            {
                try
                {
                    int Sum = 0;
                    string temp = BinData.Substring(i, 4);
                    for (int j = 0; j < temp.Length; j++)
                    {
                        if (temp[j] == '1')
                        {
                            Sum = Sum + Convert.ToInt32(Math.Pow(2, temp.Length - j - 1));
                        }
                    }
                    hexbuilder.Append(Convert.ToString(Sum, 16));
                }
                catch
                {
                    break;
                }
            }
            //Console.WriteLine("hex : " + hexbuilder.ToString());
            string hexdata = hexbuilder.ToString();
            StreamReader KeyStream = new StreamReader(KeyFileName);
            string KeyString = KeyStream.ReadToEnd();
            int Key = Convert.ToInt32(KeyString);
            int CurrentPosition = 0;
            int Length = hexdata.Length;
            if (hexdata.Length % 2 != 0 && hexdata[hexdata.Length - 1] == '0')
            {
                Length = Length - 1;
            }
            string[] CurrentCode = new string[Length / 2];
            string[] NewCode = new string[Length / 2];
            for (int i = 0; i < Length; i = i + 2)
            {
                CurrentCode[i / 2] = hexdata.Substring(i, 2);
                //Console.Write(CurrentCode[i / 2] + " ");
            }
            if (Key % 2 == 0)
            {
                CurrentPosition = 1;
            }
            for (int i = 0; i < Key; i++)
            {
                //Console.WriteLine("\nCurrentPosition " + CurrentPosition);
                for (int j = 0; j < hexdata.Length / 2; j++)
                {
                    if (CurrentPosition == 0)
                    {
                        if (j == 0)
                        {
                            NewCode[j] = CurrentCode[j];
                            //Console.Write(NewCode[j] + " ");
                        }
                        else
                        {
                            int decBeforeCurrentCode = Convert.ToInt32(CurrentCode[j - 1], 16);
                            int decCurrentCode = Convert.ToInt32(CurrentCode[j], 16);
                            NewCode[j] = (decBeforeCurrentCode ^ decCurrentCode).ToString("x");
                            //Console.Write(NewCode[j] + " ");
                        }
                    }
                    else
                    {
                        int tempj = hexdata.Length / 2 - 1 - j;
                        if (tempj == 0)
                        {
                            NewCode[j] = CurrentCode[j];
                            //Console.Write(NewCode[j] + " ");
                        }
                        else
                        {
                            int decAfterCurrentCode = Convert.ToInt32(CurrentCode[j + 1], 16);
                            int decCurrentCode = Convert.ToInt32(CurrentCode[j], 16);
                            NewCode[j] = (decAfterCurrentCode ^ decCurrentCode).ToString("x");
                            //Console.Write(NewCode[j] + " ");
                        }
                    }
                }
                //Console.WriteLine();
                CurrentPosition = 1 - CurrentPosition;

                NewCode.CopyTo(CurrentCode, 0);
            }
            StreamWriter AfterVariationsDecrypt = new StreamWriter(OutputFileName);
            //Console.Write("AfterVariationsDecrypt : ");
            foreach (string Current in CurrentCode)
            {
                if (Current.Length == 1)
                {
                    StringBuilder tempbuilder = new StringBuilder();
                    tempbuilder.Append("0");
                    tempbuilder.Append(Current);
                    AfterVariationsDecrypt.Write(tempbuilder.ToString().ToUpper());
                    //Console.Write(tempbuilder.ToString().ToUpper());
                }
                else
                {
                    AfterVariationsDecrypt.Write(Current.ToUpper());
                    //Console.Write(Current.ToUpper());
                }
            }
            //Console.WriteLine();
            AfterVariationsDecrypt.Close();
        }
        static void Main(string[] args)
        {
            int loop = 4;
            //Initial_OriginalData("OriginalData.txt", "AfterInitialOriginalData.txt");
            //Initial_HomophonicTable("HomophonicTable.txt");

            //Homophonic_Encryption("AfterInitialOriginalData.txt", "AfterHomophonicData.txt", "HomophonicTable.txt");
            //Star_Transition_Encryption("AfterHomophonicData.txt", "AfterStarTransitionData.txt");
            //Variations_Encryption("AfterStarTransitionData.txt", "AfterVariationsData.txt", "VariationsKey.txt");
            //for (int i = 0; i < loop; i++)
            //{
            //    Homophonic_Encryption("AfterVariationsData.txt", "AfterHomophonicData.txt", "HomophonicTable.txt");
            //    Star_Transition_Encryption("AfterHomophonicData.txt", "AfterStarTransitionData.txt");
            //    Variations_Encryption("AfterStarTransitionData.txt", "AfterVariationsData.txt", "VariationsKey.txt");
            //}
            Variations_Decryption("AfterVariationsData.txt", "AfterVariationsDataDecrypt.txt", "VariationsKey.txt");
            Star_Transition_Decryption("AfterVariationsDataDecrypt.txt", "AfterStarTransitionDataDecrypt.txt");
            Homophonic_Decryption("AfterStarTransitionDataDecrypt.txt", "AfterHomophonicDataDecrypt.txt", "HomophonicTable.txt");
            for (int i = 0; i < loop; i++)
            {
                Variations_Decryption("AfterHomophonicDataDecrypt.txt", "AfterVariationsDataDecrypt.txt", "VariationsKey.txt");
                Star_Transition_Decryption("AfterVariationsDataDecrypt.txt", "AfterStarTransitionDataDecrypt.txt");
                Homophonic_Decryption("AfterStarTransitionDataDecrypt.txt", "AfterHomophonicDataDecrypt.txt", "HomophonicTable.txt");
            }
        }
        public static void PrintValues(IEnumerable myList, StreamWriter afterdata)
        {
            foreach (Object obj in myList)
                afterdata.Write(obj);
        }
    }
}
