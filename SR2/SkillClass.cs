using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2
{
    public class SkillClass
    {
        public string Name { get; set; }
        public string Specialization { get; set; }
        public int Value { get; set; }

        public static int getSkill(CharacterClass c,DamageClass d)
        {
                foreach (SkillClass sk in c.skillList)
                {
                    if (sk.Name == d.skill)
                    {
                        if (sk.Specialization == "false")
                            return sk.Value;
                        else
                            return sk.Value + 2;

                    }
                }

            
            return -1;
        }

        public static string getAllSkills(CharacterClass c)
        {
            string s = string.Empty;
            foreach (DamageClass d in c.damageList)
            {
                foreach (SkillClass sk in c.skillList)
                {
                    if (sk.Name == d.skill)
                    {
                        s += sk.Name;
                        if (sk.Specialization == "false")
                           s += " ("+sk.Value + ")\r\n";
                        else
                            s+= " (" + (sk.Value + 2).ToString() + ")\r\n" ;

                    }
                }

            }
            return s;
        }
    }
}
