    using System;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GISTest
{
    using ESRI.ArcGIS.Carto;

    public partial class LayerAttrib : Form
    {
        private int nSpatialSearchMode;
        
        public LayerAttrib()
        {
            InitializeComponent();

            nSpatialSearchMode = 0;
        }

        private void LayerAttrib_Load(object sender, EventArgs e)
        {

        }
        
        // 打开属性表时激活

        public void ShowFeatureLayerAttrib(IFeatureLayer layer_IFeatureLayer)
        {
            if (layer_IFeatureLayer != null)
            {
                axMapControl1.ClearLayers();
                
                axMapControl1.AddLayer(layer_IFeatureLayer);
                
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

                dataGridView1.DataSource = featureTable_DataTable;
                
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
                            
                            // 依次读取矢量要素的每一个字段值
                            
                            for (int i = 0; i < nFieldCount; i++)
                            {
                                switch (feature.Fields.Field[i].Type)
                                {
                                    case esriFieldType.esriFieldTypeOID:

                                        featureRec[feature.Fields.Field[i].Name] = Convert.ToInt32(feature.Value[i]);
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeGeometry:

                                        featureRec[feature.Fields.Field[i].Name] = 
                                            
                                            feature.Shape.GeometryType.ToString();
                                                                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeInteger:
                                        
                                        featureRec[feature.Fields.Field[i].Name] = Convert.ToInt32(feature.Value[i]);
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeString:

                                        featureRec[feature.Fields.Field[i].Name] =

                                            feature.Value[i].ToString();
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeDouble:

                                        featureRec[feature.Fields.Field[i].Name] =

                                            Convert.ToDouble(feature.Value[i]);
                                        
                                        break;
                                }     
                            }

                            featureTable.Rows.Add(featureRec);

                            feature = cursor.NextFeature();
                        }
                        
                        if(drawshape)
                            
                            axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, Type.Missing, Type.Missing);
                    }
                }
            }
        }

        private void btAttriSearch_Click(object sender, EventArgs e)
        {
            IFeatureLayer featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;

            if (featureLayer != null)
            {
                IQueryFilter filter = new QueryFilterClass();

                filter.WhereClause = txtWhereClause.Text;
                
                ShowFeatures(featureLayer, filter, true);
            }
            
            
        }
        
        // 框选
        
        private void btBoxSearch_Click(object sender, EventArgs e)
        {
            IFeatureLayer featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;

            if (featureLayer != null)
            {
                nSpatialSearchMode = 1;
            }
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            switch (nSpatialSearchMode)
            {
                  case  1:
                      
                      IEnvelope box = axMapControl1.TrackRectangle();
                       
                      IFeatureLayer featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
                      
                      if (featureLayer != null)
                      {
                          ISpatialFilter filter = new SpatialFilterClass();

                          filter.WhereClause = txtWhereClause.Text;

                          filter.Geometry = box;
                          
                          ShowFeatures(featureLayer, filter, true);
                      }
                      
                      break;
            } 
        }

    }
}
