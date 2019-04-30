using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISTest
{
    public delegate void Render(int n);
    
    public partial class Form4 : Form
    {
        private Render _render;
        
        public Form4(Render render)
        {
            _render = render;
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt16(numericUpDown1.Text);

            _render(n);
            
            Close();
        }
    }
}
