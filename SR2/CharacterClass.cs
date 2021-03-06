﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2
{
    public class CharacterClass
    {
        public System.Windows.Forms.Label label;
        public System.Windows.Forms.ComboBox weaponList;
        public System.Windows.Forms.ComboBox modeList;
        public System.Windows.Forms.ComboBox coverCombo;
        public string Name { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Charisma { get; set; }
        public int Willpower { get; set; }
        public int Intuition { get; set; }
        public int Logic { get; set; }
        public int Body { get; set; }
        public int Reaction { get; set; }
        public int Initiative { get; set; }
        public int Armor { get; set; }
        
        public int curInitiative = 0;
        public int Edge { get; set; }
        public float Magic { get; set; }
        public int Health { get; set; }
        public int Stun { get; set; }
        public List<DamageClass> damageList = new List<DamageClass>();
        public List<SkillClass> skillList = new List<SkillClass>();

        public CharacterClass()
        {
            Strength = 0;
            Intelligence = 0;
            Charisma = 0;
            Willpower = 0;
            Intuition = 0;
            Body = 0;
            Agility = 0;
            Logic = 0;
            Reaction = 0;
            Initiative = 0;
            Armor = 0;
            Health = 10;
            Stun = 10;
        }
        public CharacterClass(int Strength = 0,
                              int Intelligence = 0,
                              int Charisma = 0,
                              int Willpower = 0,
                              int Intuition = 0,
                              int Body = 0,
                              int Reaction = 0,
                              int Initiative = 0,
                              int Armor = 0)
        {
            this.Armor = Armor;
            this.Body = Body;
            this.Charisma = Charisma;
            this.Initiative = Initiative;
            this.Intuition = Intuition;
            this.Intelligence = Intelligence;
            this.Reaction = Reaction;
            this.Strength = Strength;
            this.Willpower = Willpower;
            Health = 10;
            Stun = 10;
        }

        public string getLabel()
        {
            string s = string.Empty;
            s = Name + " A:"+Agility+ " B:"+Body+ " Str:" +Strength+ " Int:"+Intelligence+ " Intu:" + Intuition+ " C:"+Charisma+" W:"+Willpower+ " L:"+Logic+" Init:" + Initiative + " R:" + Reaction;
            return s;
        }

        public int getAttributeSkillValue(DamageClass d)
        {
            if (d.attrib == "Agility")
                return Agility;
            if (d.attrib == "Body")
                return Body;
            if (d.attrib == "Intelligence")
                return Intelligence;
            if (d.attrib == "Strength")
                return Strength;
            if (d.attrib == "Intuition")
                return Intuition;
            if (d.attrib == "Charisma")
                return Charisma;
            if (d.attrib == "Willpower")
                return Willpower;
            if (d.attrib == "Logic")
                return Logic;

            return -1;
        }
        public int getAttributeSkillValue(string attrib)
        {
            if (attrib == "Agility")
                return Agility;
            if (attrib == "Body")
                return Body;
            if (attrib == "Intelligence")
                return Intelligence;
            if (attrib == "Strength")
                return Strength;
            if (attrib == "Intuition")
                return Intuition;
            if (attrib == "Charisma")
                return Charisma;
            if (attrib == "Willpower")
                return Willpower;
            if (attrib == "Logic")
                return Logic;

            return -1;
        }
    }
}
