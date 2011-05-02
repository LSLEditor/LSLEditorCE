/*
 * @author MrSoundless
 * @date 29 April 2011
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LSLEditor
{
    public partial class llTextBoxForm : Form
    {
        private SecondLifeHost host;
        private int Channel;
        private string ObjectName;
        private string OwnerName;
        private SecondLife.key id;

        public llTextBoxForm(SecondLifeHost host, SecondLife.String strObjectName, SecondLife.key id, SecondLife.String strOwner, SecondLife.String strMessage, SecondLife.integer intChannel)
        {
            InitializeComponent();

            this.host = host;
            this.Channel = intChannel;
            this.OwnerName = strOwner;
            this.ObjectName = strObjectName;
            this.id = id;

            this.label1.Text = strMessage.ToString().Replace("&", "&&");
        }

        private void buttonIgnore_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
			Button button = sender as Button;
			if (button == null)
				return;

			host.Chat(this,this.Channel, this.OwnerName, this.id, textBox.Text.Replace("&&","&"), CommunicationType.Say);
			this.Close();
        }
    }
}