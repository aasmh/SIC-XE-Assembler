using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Term_7_project
{
     public class Program
    {
        static string Baseloc = null;
        static string[] Alllines = File.ReadAllLines(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Op codes.txt");

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
        public static string[] getdisp(string PC, string symloc)
        {
             
            string[] rvalue = new string[3];
            string sol = null;

            int num1 = Convert.ToInt32(symloc, 16);
            int num2 = Convert.ToInt32(PC, 16);
            int num3 = num1 - num2;
            if (num3 < 2048 && num3 > -2048)//pc
            {
                sol = "PC";
            }
            else//base
            {
                num2 = Convert.ToInt32(Baseloc, 16);
                num3 = num1 - num2;
                sol = "BASE";
            }

            string ans = num3.ToString("X");
            string disp = null;
            if (ans.Length == 1)
            {
                disp = "00" + ans;
            }
            else if (ans.Length == 2)
            {
                disp = "0" + ans;
            }
            else if (ans.Length >= 8)
            {
                disp = ans.Substring(ans.Length - 3, 3);
            }

            
            rvalue[0] = disp;//disp
            rvalue[1] = ans;
            rvalue[2] = sol;
            return rvalue;
        }
        public static bool checkPC(string PC,string sym)
        {

            string solution = getdisp(PC,Findloc(sym))[2];

            if(solution == "PC")
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
            else if(sym[0] == '#' || sym[0] == '@')
            {
                data = sym.Substring(1);
            }
            if (checknum(data))
            {
                return data;
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
            foreach (string line in Alllines)
            {
                string[] x = null;
                x = line.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                string Finst = null;
                if (inst[0] == '+')//format 4
                {
                    Finst = inst.Substring(1);
                }
                else if (inst[0] == '&')//format 5
                {
                    Finst = inst.Substring(1);
                }
                else if (inst[0] == '$')//format 6
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
        public static string Getformat(string inst)
        {
            foreach (string line in Alllines)
            {
                string[] x = null;
                x = line.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                string Finst = null;
                if (inst[0] == '+')//format 4
                {
                    Finst = inst.Substring(1);
                }
                else if (inst[0] == '&')//format 5
                {
                    Finst = inst.Substring(1);
                }
                else if (inst[0] == '$')//format 6
                {
                    Finst = inst.Substring(1);
                }
                else
                {
                    Finst = inst;
                }

                if (x[0] == Finst)
                {
                    return x[2];
                }

            }
            throw new Exception("Error, Not found");
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
                OP = "0000" + binum.Substring(0, 2);
                return OP;
            }
            else if (binum.Length == 3)
            {
                OP = "00000" + binum.Substring(0, 3);
                return OP;
            }
            else if (binum == "0")
            {
                OP = "000000";
                return OP;
            }
            throw new Exception("Error");
        }
        
        static void Main(string[] args)
        {
            //Thread thr = new Thread(new ThreadStart(x12));
            //thr.Start();

            string location = @"C:\Users\Home\Desktop\project-master\Term-7-project-master\sic.txt";
            List<string> sym = new List<string>();
            List<string> inst = new List<string>();
            List<string> data = new List<string>();
            List<string> loc = new List<string>();
            //string Base;
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
                        Baseloc = Findloc(data[counter]);
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
            File.WriteAllText(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Outputs\Out with MCode.txt","");

            List<string> machinecode = new List<string>();
            List<string> Pass2 = new List<string>();
            machinecode.Add("     ");
            string tempcode = null;
            for (int i = 1; i < inst.Count - 1; i++)
            {
                //string temp = null;
                int flagnum = 0;
                string thisdata = data[i]; 
                string op; string ni; string disp;
                string x = "0";
                string format = Getformat(inst[i]);

                if (format == "1")
                {
                    tempcode = Format1(inst[i]);
                }
                else if (format == "2")
                {
                    tempcode = Format2(inst[i],data[i]);
                }
                else if(format == "3/4")
                {
                    if (inst[i] == "RSUB")
                    {
                        op = FindOp(inst[i]);
                        ni = "000000";
                        disp = "000";
                        tempcode = op + "0000";
                        
                    }
                    string clean = data[i];
                    if(data[i][0] == '@' || data[i][0] == '#')
                    {
                        clean = data[i].Substring(1);
                        if (checknum(clean))
                        {
                            flagnum = 1;
                        }
                        
                    }
                    else if((data[i].Substring(data[i].Length -2) == ",X"))
                    {
                        clean = data[i].Substring(0,data[i].Length -2);
                        x = "1";
                    }
                    
                    if(inst[i][0] == '+')//format 4     32bit
                    {
                        op = Opcode(inst[i]);
                        
                        if (flagnum == 1)
                        {
                            ni = "01"+x+"001";
                            disp = Convert.ToInt32(clean).ToString("X");
                            if(disp.Length == 1)
                            {
                                disp = "0000" + disp;
                            }
                            else if (disp.Length == 2)
                            {
                                disp = "000" + disp;
                            }
                            else if (disp.Length == 3)
                            {
                                disp = "00" + disp;
                            }
                            else if (disp.Length == 4)
                            {
                                disp = "0" + disp;
                            }
                        }
                        else
                        {
                            ni = addressing(loc[i + 1], inst[i], data[i]);
                            disp = "0" + Findloc(clean);
                        }
                        tempcode = MC32(op, ni, disp);
                    }
                    else if (inst[i][0] == '&')//format 5   24bit
                    {
                        op = Opcode(inst[i]);
                        ni = addressing(loc[i + 1], inst[i], data[i]);
                        disp = getdisp(loc[i + 1], Findloc(data[i]))[0];
                        tempcode = MC32(op, ni, disp);

                    }
                    else if (inst[i][0] == '$')//format 6 32bit
                    {
                        op = Opcode(inst[i]);
                        ni = addressing(loc[i + 1], inst[i], data[i]);
                        disp = "0" + Findloc(clean);
                        tempcode = MC32(op, ni, disp);

                    }
                    else if(inst[i] != "RSUB")//format 3
                    {
                        op = Opcode(inst[i]);
                        ni = addressing(loc[i + 1], inst[i], data[i]);
                        if (flagnum == 1)
                        {

                            disp = Convert.ToInt32(clean).ToString("X");
                            if (disp.Length == 1)
                            {
                                disp = "00" + disp;
                            }
                            else if (disp.Length == 2)
                            {
                                disp = "0" + disp;
                            }
                        }
                        else
                        {
                            disp = getdisp(loc[i + 1], Findloc(data[i]))[0];
                        }
                        //disp = getdisp(loc[i + 1], Findloc(data[i]))[0];
                        tempcode = MC32(op, ni, disp);
                    }
                }
                else if(format == "0")
                {
                    if (inst[i] == "BYTE")
                    {
                        int datalen = thisdata.Length;
                        string value = thisdata.Substring(2, datalen - 3);
                        if (data[i][0] == 'c')
                        {

                            byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
                            string hex = Convert.ToHexString(asciiBytes);
                            op = hex;
                            ni = " ";
                            disp = " ";
                            tempcode = op;
                        }
                        else
                        {
                            op = value;
                            ni = " ";
                            disp = " ";
                            tempcode = op;
                        }
                    }

                    else if (inst[i] == "WORD")
                    {
                        op = int.Parse(data[i]).ToString("X");
                        ni = "";
                        disp = "";
                        tempcode = op;
                    }
                    else if (inst[i] == "RESW")
                    {
                        op = "      ";
                        ni = "";
                        disp = "";
                        tempcode = op;
                    }
                    else if (inst[i] == "RESB")
                    {
                        op = "      ";
                        ni = "";
                        disp = "";
                        tempcode = op;
                    }
                    else if (inst[i] == "BASE")
                    {
                        op = "      ";
                        ni = "";
                        disp = "";
                        tempcode = op;
                    }
                }
                
                //add if statment for each, format to send to the function.

                /*
                if (inst[i] == "RSUB")
                {
                    op = Opcode(inst[i]);
                    ni = "000000";
                    disp = "000";
                    tempcode = MC32(op, ni, disp);
                }

                else 
                /*
                else
                {
                    op = Opcode(inst[i]);
                    ni = addressing(loc[i + 1], inst[i], data[i]);
                    disp = getdisp(loc[i + 1],Findloc( data[i]))[0];//find location sends the data location place
                    Fop = op + ni;
                    string test = Convert.ToInt32(Fop, 2).ToString("X");
                }
                */
                else
                {
                     op = Opcode(inst[i]);
                     ni = addressing(loc[i+1], inst[i], data[i]);
                     disp = getdisp(loc[i + 1], Findloc(data[i]))[0];
                    tempcode = MC32(op, ni, disp);
                }
                machinecode.Add(tempcode);
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",loc[i],sym[i],inst[i],data[i],machinecode[i],i);
                
                
                
                string temp = "" +loc[i] +"\t" + sym[i] + "\t" + inst[i] + "\t" + data[i] + "\t" + machinecode[i];
                Pass2.Add(temp);
            }

            File.AppendAllLines(@"C:\Users\Home\Desktop\project-master\Term-7-project-master\Outputs\Out with MCode.txt", Pass2);
            
            /*
            for (int i = 0; i < inst.Count; i++)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}",loc[i],sym[i],inst[i],data[i]);
                
            }
            */

            

        }

        public static bool checknum(string data)
        {
            char[] Char = data.ToCharArray();
            int flag= 0;
            foreach(char c in Char)
            {
                if (!char.IsDigit(c))
                {
                    flag = 1;
                }
            }
            if(flag == 0)
            {
                return true;
            }
            else
            {
                return false; 
            }

        }
        public static string MC32(string OP, string nixbpe, string disp)
        {
            string full = OP + nixbpe;

            string hexa = Convert.ToInt32(full, 2).ToString("X");

            string MC = hexa + disp;

            if (hexa.Length == 2)
            {
                MC = "0" + MC;
            }

            return MC;
        }
        public static string Format1(string inst)
        {
            return FindOp(inst);
        }

        public static string Format2(string inst,string data)
        {
            string op = FindOp(inst);
            string[] x = data.Split(',');

            if(x.Length == 2)
            {
                switch (x[0])
                {
                    case "A":
                        op += "0";
                        break;

                    case "X":
                        op += "1";
                        break;

                    case "S":
                        op += "4";
                        break;

                    case "T":
                        op += "5";
                        break;
                }
                switch (x[1])
                {
                    case "A":
                        op += "0";
                        break;

                    case "X":
                        op += "1";
                        break;

                    case "S":
                        op += "4";
                        break;

                    case "T":
                        op += "5";
                        break;
                }
            }
            else if (x.Length == 1)
            {
                switch (x[0])
                {
                    case "A":
                        op += "0";
                        break;

                    case "X":
                        op += "1";
                        break;

                    case "S":
                        op += "4";
                        break;

                    case "T":
                        op += "5";
                        break;
                }
                op += "0";
            }
            

            return op;
        }

        public static string addressing(string PC, string inst, string data)//nixbpe
        {
            int format = 0;
            char n, i, x, b, p, e;
            int flagni = 0;


            if (inst[0] == '+')//checks for format 4
            {
                e = '1';
                format = 4;
            }
            else if (inst[0] == '&')//format 5
            {
                string dataloc = Findloc(data);
                string displacement = getdisp(PC, dataloc)[1];
                int disp = Convert.ToInt32(displacement,16);

                if (disp == 0)
                {
                    e = '1';
                }
                else
                {
                    e = '0';
                }

                format = 5;
            }
            else if (inst[0] == '$')//format 6
            {
                format = 6;
                if(Baseloc == Findloc(data))
                {
                    e = '0';
                }
                else
                {
                    e = '1';
                }
            }
            else
            {
                e = ' ';
                format = 3;
            }


            int length = data.Length;

            if (data.Substring(length - 2) == ",X")// Checks for X
            {
                x = '1';
            }
            else
            {
                x = '0';
            }
            if (format != 5)
            {
                if (data[0] == '#')//checks for i
                {
                    i = '1';
                    n = '0';
                    if (!checknum(data.Substring(1)))
                    {
                        flagni = 3;
                    }
                }
                else if (data[0] == '@')//checks for n
                {
                    i = '0';
                    n = '1';
                    flagni = 3;
                }
                else    //If both N and I do not exist
                {
                    i = '1';
                    n = '1';
                    flagni = 1;
                }
            }
            else
            {
                string dataloc = Findloc(data);
                string displacement = getdisp(PC, dataloc)[1];
                int disp = Convert.ToInt32(displacement, 16);
                if (disp % 2 == 0)//displacment is even
                {
                    n = '1';
                }
                else { n = '0'; }

                if (disp > 0)
                {
                    i = '0';
                }
                else
                {
                    i = '1';
                }
            }
            if ((format == 3 || format == 5 )&& (flagni == 1 || flagni == 3))
            {
                if (checkPC(PC, data))
                {
                    p = '1';
                    b = '0';
                }
                else
                {
                    p = '0';
                    b = '1';
                }
            }
            else if ((format == 4) && (flagni == 1 || flagni == 3))
            {
                p = '0';
                b = '0';
            }
            else if(format == 6)
            {
                int address = Convert.ToInt32(Findloc(data),16);
                if (address%2== 0)
                {
                    b = '0';
                }
                else
                {
                    b= '1';
                }

                if (address == 0)
                {
                    p= '0';
                }
                else
                {
                    p = '1';
                }
            }
            else if (flagni == 0 && format == 5)
            {
                if (checkPC(PC, data))
                {
                    p = '1';
                    b = '0';
                }
                else
                {
                    p = '0';
                    b = '1';
                }
            }
            else if(flagni == 0)
            {
                b = '0';
                p = '0';
            }
            else
            {
                p = ' ';
                b = ' ';
            }
            if (format == 3)
            {
                e = '0';
            }



            string nixbpe = "" + n + i + x + b + p + e;

            return nixbpe;
        }
    }
}
