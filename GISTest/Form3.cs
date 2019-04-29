using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace GISTest
{ 
    public delegate bool AddField(string fieldName, esriFieldType filedType, int fieldLength);
    
    public partial class Form3 : Form
    {

        private AddField _addField;

        private AxMapControl axMapControl;

        private DataGridView dataGridView;
        
        public Form3(AddField addField)
        {
            this._addField = addField;
            
            InitializeComponent();             
        }
        
        public void ShowFeatureLayerAttrib(IFeatureLayer layer_IFeatureLayer)
        {
            if (layer_IFeatureLayer != null)
            {
                axMapControl.ClearLayers();
                
                axMapControl.AddLayer(layer_IFeatureLayer);
                
                DataTable  featureTable_DataTable = new DataTable();
                
                // 矢量要素图层包含的要素类
                
                IFeatureClass fcLayer_IFeatureClass = layer_IFeatureLayer.FeatureClass;
                
                // 要素类中包含的属性字段总数

                int nFieldCount = fcLayer_IFeatureClass.Fields.FieldCount;

                for (int i = 0; i < nFieldCount; i++)
                {
                    DataColumn field_DataColumn = new DataColumn {ColumnName = fcLayer_IFeatureClass.Fields.Field[i].Name};
                    
                    switch (fcLayer_IFeatureClass.Fields.Field[i].Type)
                    {
                          case  esriFieldType.esriFieldTypeOID:
                              
                              field_DataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                              
                          
                          case esriFieldType.esriFieldTypeGeometry:

                              field_DataColumn.DataType = Type.GetType("System.String");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeInteger:
                              
                              field_DataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeSingle:
                              
                              field_DataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeSmallInteger:
                              
                              field_DataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeString:
                              
                              field_DataColumn.DataType = Type.GetType("System.String");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeDouble:
                              
                              field_DataColumn.DataType = Type.GetType("System.Double");
                              
                              break;
                    }

                    featureTable_DataTable.Columns.Add(field_DataColumn);
                }

                dataGridView.DataSource = featureTable_DataTable;
                
            }

            else
            {
                axMapControl.ClearLayers();
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string fieldName = textBox1.Text;

            this._addField(fieldName, esriFieldType.esriFieldTypeString, 12);
            
            this.Close();
            

        }
       
    }
}
