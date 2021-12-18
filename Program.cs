using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Term_7_project
{
    internal class Program
    {
        public static string AddHex(string In,int x)
        {
            string Out =null;
            int decValue = Convert.ToInt32(In, 16); // hex to decimal
            int ans = decValue + x;
            Out = ans.ToString("X");//decimal to hex
            return  Out;
        }
        public static string hextobin(string hex)
        {
            string binum;
            binum = Convert.ToString(Convert.ToInt32(hex,16),2);
            return binum;
        }
        public static string getdisp(string PC, string sym)
        {
            
            int num1 = Convert.ToInt32(Findloc(sym), 16);
            int num2 = Convert.ToInt32(PC, 16);
            int num3 = num1 - num2;


            string ans = num3.ToString("X");

            return ans;
        }
        public static bool checkPC(string PC,string sym)
        {

            int num3 = Convert.ToInt32(getdisp(PC, sym), 16);

            if(num3 < 2048 && num3 > -2048)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        public static string Findloc(string sym)
        {
            string data = sym;

            if (sym.Substring(sym.Length - 2, 2) == ",X")//checks for X, if true removes the ",X"
            {
                data = sym.Substring(0, sym.Length - 2);

            }
            string[] Alllines = File.ReadAllLines(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Outputs\Symbol table.txt");
            foreach(string line in Alllines)
            {
                string[] x = null;
                x = line.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                string test = x[1].Trim();
                
                
                if (test == data)
                {
                    return x[0];
                }
                

            }
                throw new Exception("Error, Not found");
        }
        public static string FindOp(string inst)
        {
            string[] Alllines = File.ReadAllLines(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Op codes.txt");
            foreach (string line in Alllines)
            {
                string[] x = null;
                x = line.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                string Finst = null;
                if(inst[0] == '+')
                {
                    Finst = inst.Substring(1);
                }
                else
                {
                    Finst = inst;
                }

                if (x[0] == Finst)
                {
                    return x[1];
                }
                
            }
            throw new Exception("Error, Not found");
        }
        public static string addressing(string PC,string inst, string data)//nixbpe
        {
            
            char n, i, x, b, p, e;
            
            if (inst[0] == '+')//checks for format 4
            {
                e = '1';
            }
            else if(inst[0] == '&')//format 5
            {
                e = '0';
            }
            else
                e = '0';
            int length = data.Length;

            if (data.Substring(length - 2, 2) == ",X")// Checks for X
            {
                x = '1';
            }
            else
                x = '0';

            if (data[0] == '#')//checks for i
            {
                i = '1';
                n = '0';
            }
            else if (data[0] == '@')//checks for n
            {
                i = '0';
                n = '1';
            }
            else    //If both N and I do not exist
            {
                i = '1';
                n = '1';
            }

            if( e == '1')
            {
                p = '0';
                b = '0';
                
            }
            else if (checkPC(PC,data))
            {
                p = '1';
                b = '0';
            }
            else
            {
                p = '0';
                b = '1';
            }


            string nixbpe = ""+n + i + x + b + p + e;
            return nixbpe;
        }
        public static string Opcode(string inst)
        {
            string hex = FindOp(inst);
            
            string binum;
            string OP;
            binum = Convert.ToString(Convert.ToInt32(hex, 16), 2);
            if (binum.Length == 8)
            {
                OP = binum.Substring(0, 6);
                return OP;
            }
            else if (binum.Length == 7)
            {
                OP = "0" + binum.Substring(0, 5);
                return OP;
            }
            else if (binum.Length == 6)
            {
                OP = "00" + binum.Substring(0, 4);
                return OP;
            }
            else if (binum.Length == 5)
            {
                OP = "000" + binum.Substring(0, 3);
                return OP;
            }
            else if (binum.Length == 4)
            {
                OP = "0000" + binum.Substring(0, 3);
                return OP;
            }
            else if (binum.Length == 3)
            {
                OP = "00000" + binum.Substring(0, 3);
                return OP;
            }
            else if( binum == "0")
            {
                OP = "000000";
                return OP;
            }
            throw new Exception("Error");
        }
        /*
        static void x12()
        {
            Console.WriteLine("Hai");
        }
        */
        static void Main(string[] args)
        {
            //Thread thr = new Thread(new ThreadStart(x12));
            //thr.Start();

            string location = @"C:\Users\Home\Desktop\project-master\Term-7-project-master\sic.txt";
            List<string> sym = new List<string>();
            List<string> inst = new List<string>();
            List<string> data = new List<string>();
            List<string> loc = new List<string>();
            string Base;
            int lcount = 0;

            //string test = "18";
            //Console.WriteLine(hextobin(test));

            if (File.Exists(location))
            {
                string[] lines = File.ReadAllLines(location);
                foreach (string line in lines)
                {
                    string[] x = null;
                    x = line.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                    if (x.Length == 3)
                    {
                        sym.Add(x[0]);
                        inst.Add(x[1]);
                        data.Add(x[2]);
                    }
                    else if (x.Length == 2)
                    {
                        sym.Add("     ");
                        inst.Add(x[0]);
                        data.Add(x[1]);
                    }
                    else if(x.Length == 1)
                    {
                        sym.Add("     ");
                        inst.Add(x[0]);
                        data.Add("     ");
                    }

                    
                    
                    
                }
            }
            loc.Insert(0, data[0]);//Takes the first entry on the memory column and places it as the starting location counter
            for (int counter = 0; counter < inst.Count-1; counter++)
            {
                string temp = inst[counter].ToString();
                char[] chars = temp.ToCharArray();
                switch (inst[counter]) //17 cases where the format isn't 3/4
                {
                    case "ADDR":
                        lcount = 2;
                        break;
                    case "BASE":
                        lcount = 0;
                        Base = data[counter];
                        break;

                    case "CLEAR":
                        lcount = 2;
                        break;

                    case "COMPR":
                        lcount = 2;
                        break;

                    case "DIVR":
                        lcount = 2;
                        break;

                    case "FIX":
                        lcount = 1;
                        break;
                    
                    case "FLOAT":
                        lcount = 1;
                        break;
                    
                    case "HIO":
                        lcount = 1;
                        break;
                    
                    case "MULR":
                        lcount = 2;
                        break;
                    
                    case "NORM":
                        lcount = 1;
                        break;
                    
                    case "RMO":
                        lcount = 2;
                        break;
                    
                    case "SHIFTL":
                        lcount = 2;
                        break;
                    
                    case "SHIFTR":
                        lcount = 2;
                        break;
                    
                    case "SIO":
                        lcount = 1;
                        break;
                    
                    case "SUBR":
                        lcount = 2;
                        break;
                    
                    case "SVC":
                        lcount = 2;
                        break;
                    
                    case "TIO":
                        lcount = 1;
                        break;
                    
                    case "TIXR":
                        lcount = 2;
                        break;
                    case "BYTE":
                         temp = data[counter].ToString();
                        
                        chars = temp.ToCharArray();
                        if (chars[0] == 'X')
                        {
                           lcount = (chars.Length -3)/2 ;
                        }
                        else if (chars[0] == 'c')
                        {
                            lcount = chars.Length - 3;
                        }
                        break;
                    case "RESW":
                        lcount = Convert.ToInt32(data[counter]) * 3;
                        break;
                    case "RESB":
                        lcount = Convert.ToInt32(data[counter].ToString());
                        break;
                    
                        


                    default:
                        if(chars[0]== '+')
                        {
                            lcount = 4;
                        }
                        else if (chars[0] == '&')
                        {
                            lcount = 3;
                        }
                        else if (chars[0] == '$')
                        {
                            lcount = 4;
                        }
                        else
                        lcount = 3;
                        break;
                }
                if(counter == 0 )
                {
                    loc.Insert(1,data[0]);
                }
                else
                loc.Insert(counter + 1,AddHex(loc[counter].ToString(), lcount));//add Location counter via method
                
            }
            //Outputs
            List<string> Symtb = new List<string>();
            List<string> Lines = new List<string>();
            for (int i =1;i<inst.Count-1;i++)
            {
                string temp1 = loc[i] + "\t" + sym[i] + "\t" + inst[i] + "\t" + data[i];
                Lines.Add(temp1);
                if (sym[i] != "     ")
                {
                    string temp = loc[i] +"\t"+ sym[i];
                    Symtb.Add(temp);
                }
            }
            int size = inst.Count;
            string end = "\nThe End location is :" + loc[size-1];
            File.WriteAllLines(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Outputs\Symbol table.txt", Symtb);
            File.WriteAllLines(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Outputs\Out.txt", Lines);
            File.AppendAllText(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Outputs\Out.txt", end);



            ///////////////////pass 2///////////////////


            
            for (int i = 1; i < inst.Count - 1; i++)
            {
                //string temp = null;
                string Fop = null;
                string thisdata = data[i]; 
                string op; string ni; string disp;
                if (inst[i] == "RSUB")
                {
                    op = Opcode(inst[i]);
                    ni = "000000";
                    disp = "000";

                }
                else if (inst[i] == "BYTE")
                {
                    int datalen = thisdata.Length;
                    string value = thisdata.Substring(2, datalen- 3);
                    if (data[i][0] == 'c')
                    {

                        byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
                        string hex = Convert.ToHexString(asciiBytes);
                        op = hex;
                        ni = "";
                        disp = "";
                    }
                    else
                    {
                        op = value;
                        ni = "";
                        disp = "";
                    }
                    
                }
                else if(inst[i] == "WORD")
                {
                    op = int.Parse(data[i]).ToString("X");
                    ni = "";
                    disp = "";
                }
                else if (inst[i] == "RESW")
                {
                    op = "";
                    ni = "";
                    disp = "";
                }
                else if (inst[i] == "RESB")
                {
                    op = "";
                    ni = "";
                    disp = "";
                }
                else
                {
                    op = Opcode(inst[i]);
                    ni = addressing(loc[i + 1], inst[i], data[i]);
                    disp = getdisp(loc[i + 1], data[i]);
                    Fop = op + ni;
                    string test = Convert.ToInt32(Fop, 2).ToString("X");
                }
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",op,ni,disp,inst[i],i);
                
                
                
                //temp = "" +loc[i] +"\t" + sym[i] + "\t" + inst[i] + "\t" + data[i];
                File.AppendAllText(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Outputs\Out with MCode.txt", end);
            }

            /*
            for (int i = 0; i < inst.Count; i++)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}",loc[i],sym[i],inst[i],data[i]);
                
            }
            */



        }
    }
}
