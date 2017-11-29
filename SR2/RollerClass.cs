using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2
{
    class RollerClass
    {
        public int numDice;
        public int add = 0;
        List<int> rolls = new List<int>();
        public int Initative = 0;
        Random die;

        public RollerClass()
        {
            numDice = 0;
            add = 0;
            die = new Random();
        }

        public RollerClass(int numDice = 0,int add = 0)
        {

        }

        public List<int> Roll()
        {
            rolls = new List<int>();
            List<int> allRolls = new List<int>();
            
            
            for(int i = 0;i < numDice;i++)
            {
                allRolls.Add(die.Next(5) + 1);
            }
            if (add != 0)
            {
                allRolls.Add(add);
                Initative = allRolls.Take(allRolls.Count).Sum();
            }
            rolls = allRolls;
            return allRolls;
        }

        public List<int> Roll(int numDice,int add)
        {
            this.numDice = numDice;
            this.add = add;
            return Roll();
        }

        public string displayInitiative(int numDice, int add)
        {
            string s = string.Empty;
            s = "("+string.Join(",",Roll(numDice, add)) +") ("+Initative+")";
            return s;
        }

        public string displayRolls(int attrib,int numDice)
        {
            string s = string.Empty;
            s = "(" + string.Join(",", Roll(attrib+numDice, 0)) + ")";
            return s;
        }
    }
}
