using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2
{
    public class DamageClass
    {
        public string Name { get; set; }
        public int baseDamage { get; set; }
        public bool isPhysical { get; set; }
        public int armorPen { get; set; }
        public bool fullAuto { get; set; }
        public bool semi { get; set; }
        public bool single { get; set; }
        public bool melee { get; set; }
        public bool spell { get; set; }
        public int modifier { get; set; }
        public string attrib { get; set; }
        public string skill { get; set; }
        public string special { get; set; }

        public DamageClass()
        {
            baseDamage = 0;
            isPhysical = true;
            armorPen = 0;
            fullAuto = false;
            single = false;
            semi = false;
            melee = false;
            spell = false;
            modifier = 0;
            attrib = string.Empty;
        }

        public static DamageClass getActiveWeapon(CharacterClass c)
        {
            foreach (DamageClass dam in c.damageList)
            {
                if (dam.Name == c.weaponList.SelectedItem.ToString())
                {
                    return dam;
                }
            }
            return null;
        }

    }
}
