using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DnDBattleSim.Classes;
using DnDBattleSim.Classes.SubClasses;

namespace DnDBattleSim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Random randomizer = new Random();


            BattleField Battlefield = new BattleField(randomizer,4);
            Character ally = new Character();
            ally.Name = "ally";
            Character ally2 = new Character();
            ally.Name = "ally";
            Character ally3 = new Character();
            ally.Name = "ally";
            Character enemy = new Character();
            enemy.Name = "enemy";
            Battlefield.addCharacter(ally,3,2);
            //Battlefield.addCharacter(ally2, 6, 4);
            //Battlefield.addCharacter(ally3, 6, 5);
            Battlefield.addCharacter(enemy,1,3);
            string msg = "";
            foreach (Character chara in Battlefield.getCharacters())
            {
                msg += chara.Name + " x: " + chara.getPoint().X + " y:" + chara.getPoint().Y + System.Environment.NewLine;
            }
            List<DnDBattleSim.Classes.SubClasses.Point> resultmove = ally.MoveTo(enemy.getPoint());
        }

        private void goal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
