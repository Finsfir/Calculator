using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Expression
    {
        private int target;
        public int Target
        {
            get => target;
            set
            {
                if (value > 0)
                    target = value;
            }
        }

        public List<string> exprQueue = new List<string>();

        public double Result(string str)
        {
            for(int pos = 1; str.Length > pos; pos++)
            {
                if (Char.IsDigit(str[pos - 1]) && str[pos] == '(')
                    str = str.Insert(pos, "*");
            }

            string forSafe = str;
            for (int pos = 0; str.Length > pos; pos++)
            {
                if (str[pos] == '/' || str[pos] == '*')
                {
                    if (Char.IsDigit(str[pos - 1]) && Char.IsDigit(str[pos + 1]))
                    {
                        int i = -1;
                        while (pos + i >= 0 && (Char.IsDigit(str[pos + i]) || (str[pos + i] == ',')))
                            i--;
                        str = str.Insert(pos + i + 1, "(");
                        pos++;
                        i = 1;
                        while (str.Length > pos + i && (Char.IsDigit(str[pos + i]) || (str[pos + i] == ',')))
                            i++;
                        if (str.Length > pos + i)
                            str = str.Insert(pos + i, ")");
                        else
                            str += ")";
                    }
                }
            }
            double res = Solve(str, 0);

            exprQueue.Add(forSafe + " = " + res.ToString());
            return res;
        }

        private double Solve(string str, int pos)
        {
            string a = "";
            while (str.Length > pos && (Char.IsDigit(str[pos]) || (str[pos] == ',')))
            {
                a += str[pos];
                pos++;
            }
            if (str.Length == pos && a == "")
                return 0;
            else if (str.Length == pos)
                return double.Parse(a);
            switch (str[pos])
            {
                case '(':
                    int i = 0;
                    int brackets = 1;
                    while (str.Length > (pos + i) && brackets > 0)
                    {
                        i++;
                        if (str[pos + i] == ')')
                            brackets--;
                        if (str[pos + i] == '(')
                            brackets++;
                    }
                    i++;
                    if (str.Length <= pos + i)
                        return Solve(str, pos + 1);
                    else
                        switch (str[pos + i])
                        {
                            case '+':
                                return Solve(str, pos + 1) + Solve(str, pos + i + 1);
                            case '-':
                                return Solve(str, pos + 1) - Solve(str, pos + i + 1);
                            case '*':
                                return Solve(str, pos + 1) * Solve(str, pos + i + 1);
                            case '/':
                                return Solve(str, pos + 1) / Solve(str, pos + i + 1);
                            case ')':
                                return Solve(str, pos + 1);
                            default:
                                throw new Exception("Invalid input");
                        }
                    break;
                case ')':
                    if (a != "")
                        return double.Parse(a);
                    else
                        throw new Exception("Misplaced brackets");
                case '+':
                    if (a != "")
                        return double.Parse(a) + Solve(str, pos + 1);
                    else
                        throw new Exception("Misplaced '+'-operation");
                case '-':
                    if (a != "")
                        return double.Parse(a) - Solve(str, pos + 1);
                    else
                        throw new Exception("Misplaced '-'-operation");
                case '*':
                    if (a != "")
                        return double.Parse(a) * Solve(str, pos + 1);
                    else
                        throw new Exception("Misplaced '*'-operation");
                case '/':
                    if (a != "")
                        return double.Parse(a) / Solve(str, pos + 1);
                    else
                        throw new Exception("Misplaced '/'-operation");
            }
            return 0;
        }

        public override string ToString()
        {
            return exprQueue[target];
        }
    }

}
