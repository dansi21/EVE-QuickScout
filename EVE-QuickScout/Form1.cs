using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Options;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models;
using ESI.NET.Logic;

namespace EVE_QuickScout
{
    public partial class Form1 : Form
    {
        EVEEsiInformation esi;

        public Form1()
        {
            esi = new EVEEsiInformation();
            InitializeComponent();
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SystemInfo results = await esi.SearchSystemName(textBox1.Text);
            if (results == null)
            {
                label1.Text = "Not Found";
                label2.Text = "Planets: ";
                label3.Text = "Moons: ";
                label4.Text = "Belts: ";
            }
            else {
                label1.Text = results.SystemName;
                label2.Text = "Planets: " + results.Planets.ToString();
                label3.Text = "Moons: " + results.Moons.ToString();
                label4.Text = "Belts: " + results.Belts.ToString();
                label5.Text = "Kills: " + results.Kills.ToString();
                label6.Text = "NPC Kills: " + results.NPCKills.ToString();
            }
        }
    }
}
