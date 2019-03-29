using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;
using ZeroGServer;
using ZeroGServer.PlayerManagement;

namespace ZeroGGuiServer
{
    //The pictures used in the track selection, idk how to add them to source control, so theres a zip file with all of them at https://www.mediafire.com/file/faoh136bm1ouaho/level_selection_pictures.zip/file
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NetworkInterface[] allInterfaces;
        Server server;
        NewPlayerInfo npi;
        Players players;
        PlayerManager pManager;
        int maxPlayers = 2;
        string levelName;
        bool isReverse;
        int serverPort = 6001;
        Thread serverThread;
        private void Form1_Load(object sender, EventArgs e)
        {
            allInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface eachInterface in allInterfaces)
            {
                comboBox3.Items.Add(eachInterface.Name);
            }
        }
        
        

        public void StartServer(int maxPlayers, string levelName, bool isReverse, int port)
        {
            server = new Server();
            npi = new NewPlayerInfo();
            players = new Players();
            InstanceKeeper.players = players;
            pManager = new PlayerManager(server, players, levelName, isReverse, maxPlayers);
            InstanceKeeper.playerManager = pManager;
            server.Start(npi, port);
        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

      

       
        

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (button1.Text == "Start Server")
            {
                try
                {
                    serverThread = new Thread(delegate () { StartServer(maxPlayers, levelName, isReverse, serverPort); });
                    serverThread.Start();
                    button1.Text = "Stop Server";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while starting: " + ex.ToString());
                }
            }
            else if (button1.Text == "Stop Server")
            {
                try
                {
                    serverThread.Abort();
                    button1.Text = "Start Server";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.ToString());
                }
            }
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Forward")
            {
                isReverse = false;
            }
            if (comboBox2.SelectedItem.ToString() == "Reverse")
            {
                isReverse = true;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\" + comboBox1.SelectedItem.ToString() + ".png"))
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\" + comboBox1.SelectedItem.ToString() + ".png");
            }
            else if (File.Exists(Application.StartupPath + "\\" + comboBox1.SelectedItem.ToString() + ".jpg"))
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\" + comboBox1.SelectedItem.ToString() + ".jpg");
            }
            if (comboBox1.SelectedItem.ToString() == "Skydome City (Urban)")
            {
                levelName = "Urban_01";
            }
            else if (comboBox1.SelectedItem.ToString() == "Drifters Paradise (Urban)")
            {
                levelName = "Urban_02";
            }
            else if (comboBox1.SelectedItem.ToString() == "Spiral Drop (Urban)")
            {
                levelName = "Urban_03";
            }
            else if (comboBox1.SelectedItem.ToString() == "Vengo canyon (Desert)")
            {
                levelName = "Desert_01";
            }
            else if (comboBox1.SelectedItem.ToString() == "Nexus Wastes (Desert)")
            {
                levelName = "Desert_02";
            }
            else if (comboBox1.SelectedItem.ToString() == "Serpentine Valley (Desert)")
            {
                levelName = "Desert_03";
            }
            else if (comboBox1.SelectedItem.ToString() == "Nevelmale Pass (Arctic)")
            {
                levelName = "Arctic_01";
            }
            else if (comboBox1.SelectedItem.ToString() == "Northern Ring (Arctic)")
            {
                levelName = "Arctic_02";
            }
            else if (comboBox1.SelectedItem.ToString() == "Polar Cavern (Arctic)")
            {
                levelName = "Arctic_03";
            }
            else if (comboBox1.SelectedItem.ToString() == "Blue Marble (Space)")
            {
                levelName = "Space_01";
            }
            else if (comboBox1.SelectedItem.ToString() == "Trappist Station (Space)")
            {
                levelName = "Space_02";
            }
            else if (comboBox1.SelectedItem.ToString() == "Cryon Terminal (Space)")
            {
                levelName = "Space_03";
            }
            else if (comboBox1.SelectedItem.ToString() == "Michael's Bay (Island)")
            {
                levelName = "DLC_01";
            }
            else if (comboBox1.SelectedItem.ToString() == "Nososs Grill (Island)")
            {
                levelName = "DLC_02";
            }
            else if (comboBox1.SelectedItem.ToString() == "Square Creek (Island)")
            {
                levelName = "DLC_03";
            }
            else if (comboBox1.SelectedItem.ToString() == "Scorching Testinggrounds (Practice)")
            {
                levelName = "Tutorial";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            int temp = 0;
            if (int.TryParse(textBox2.Text, out temp))
            {
                serverPort = temp;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            int temp = 0;
            if (int.TryParse(textBox1.Text, out temp))
            {
                maxPlayers = temp;
            }
        }
    }
}
