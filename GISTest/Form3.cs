using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GISTest
{
    public delegate bool AddField(string fieldName, esriFieldType filedType, int fieldLength);

    public partial class Form3 : Form
    {
        private readonly AddField _addField;


        public Form3(AddField addField)
        {
            _addField = addField;

            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string fieldName = textBox1.Text;

            string fieldType = comboBox1.Text;

            int fieldSize = Convert.ToInt16(numericUpDown1.Text);

            switch (fieldType)
            {
                case "string":

                    _addField(fieldName, esriFieldType.esriFieldTypeString, fieldSize);

                    break;

                case "int":

                    _addField(fieldName, esriFieldType.esriFieldTypeInteger, fieldSize);

                    break;

                case "double":

                    _addField(fieldName, esriFieldType.esriFieldTypeDate, fieldSize);

                    break;
            }


            Close();
        }
    }
}