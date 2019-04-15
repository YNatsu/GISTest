using System;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GISTest
{
    using ESRI.ArcGIS.Carto;

    public partial class LayerAttrib : Form
    {
        
        
        public LayerAttrib()
        {
            InitializeComponent();
            
        }

        private void LayerAttrib_Load(object sender, EventArgs e)
        {

        }
        
        // 打开属性表时激活

        public void ShowFeatureLayerAttrib(IFeatureLayer layer)
        {
            if (layer != null)
            {
                axMapControl1.ClearLayers();
                axMapControl1.AddLayer(layer);
                
                DataTable  featureTable = new DataTable();
                
                // 矢量要素图层包含的要素类
                
                IFeatureClass fcLayer = layer.FeatureClass;
                
                // 要素类中包含的属性字段总数

                int nFieldCount = fcLayer.Fields.FieldCount;

                for (int i = 0; i < nFieldCount; i++)
                {
                    DataColumn field = new DataColumn {ColumnName = fcLayer.Fields.Field[i].Name};
                    
                    switch (fcLayer.Fields.Field[i].Type)
                    {
                          case  esriFieldType.esriFieldTypeOID:
                              
                              field.DataType = Type.GetType("System.Int32");
                              
                              break;
                              
                          
                          case esriFieldType.esriFieldTypeGeometry:

                              field.DataType = Type.GetType("System.String");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeInteger:
                              
                              field.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeSingle:
                              
                              field.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeSmallInteger:
                              
                              field.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeString:
                              
                              field.DataType = Type.GetType("System.String");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeDouble:
                              
                              field.DataType = Type.GetType("System.Double");
                              
                              break;
                    }

                    featureTable.Columns.Add(field);
                }

                dataGridView1.DataSource = featureTable;
                
            }

            else
            {
                axMapControl1.ClearLayers();
            }
        }

        public void ShowFeatures(IFeatureLayer featureLayer, IQueryFilter condition, bool drawshape)
        {
            if (featureLayer != null)
            {
                // 矢量要素图层关联的要素类
                
                IFeatureClass featureClass = featureLayer.FeatureClass;

                IFeatureCursor cursor;

                try
                {
                    cursor = featureClass.Search(condition, false);
                }
                catch (Exception)
                {
                    cursor = null;
                }

                if (cursor != null)
                {
                    axMapControl1.Refresh(
                        esriViewDrawPhase.esriViewGeoSelection, Type.Missing, Type.Missing
                        );
                    
                    IFeatureSelection featureSelection = featureLayer as IFeatureSelection;

                    if (featureSelection != null)
                    {
                        featureSelection.Clear();

                        DataTable featureTable = (DataTable) dataGridView1.DataSource;

                        featureTable.Rows.Clear();

                        IFeature feature = cursor.NextFeature();

                        while (feature != null)
                        {
                            if (drawshape)
                            {
                                featureSelection.Add(feature);
                            }

                            DataRow featureRec = featureTable.NewRow();

                            int nFieldCount = feature.Fields.FieldCount;
                        }
                    }
                }
            }
        }
    }
}
