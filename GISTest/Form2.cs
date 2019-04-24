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

        public void ShowFeatures(IFeatureLayer featureLayer_IFeatureLayer, IQueryFilter condition_IQueryFilter, bool drawshape)
        {
            
            if (featureLayer_IFeatureLayer != null)
            {
                // 矢量要素图层关联的要素类
                
                // IFeatureClass => IFeatureLayer.FeatureClass
                
                IFeatureClass featureClass_IFeatureClass = featureLayer_IFeatureLayer.FeatureClass;
                
                // IFeatureCursor 变量用于获取查询结果
                
                // IFeatureCursor => IFeatureClass.Search(...)
                
                IFeatureCursor cursor_IFeatureCursor;

                try
                {
                    cursor_IFeatureCursor = featureClass_IFeatureClass.Search(condition_IQueryFilter, false);
                }
                catch (Exception)
                {
                    cursor_IFeatureCursor = null;
                }

                if (cursor_IFeatureCursor != null)
                {
                    axMapControl1.Refresh(
                        esriViewDrawPhase.esriViewGeoSelection, Type.Missing, Type.Missing
                        );
                    
                    // IFeatureSelection =>  IFeatureLayer
                    
                    IFeatureSelection featureSelection_IFeatureSelection = featureLayer_IFeatureLayer as IFeatureSelection;

                    if (featureSelection_IFeatureSelection != null)
                    {
                        featureSelection_IFeatureSelection.Clear();

                        DataTable featureTable_DataTable = (DataTable) dataGridView1.DataSource;

                        featureTable_DataTable.Rows.Clear();

                        IFeature feature_IFeature = cursor_IFeatureCursor.NextFeature();

                        while (feature_IFeature != null)
                        {
                            if (drawshape)
                            {
                                featureSelection_IFeatureSelection.Add(feature_IFeature);
                            }

                            DataRow featureRec_DataRow = featureTable_DataTable.NewRow();

                            int nFieldCount = feature_IFeature.Fields.FieldCount;
                            
                            // 依次读取矢量要素的每一个字段值
                            
                            for (int i = 0; i < nFieldCount; i++)
                            {
                                switch (feature_IFeature.Fields.Field[i].Type)
                                {
                                    case esriFieldType.esriFieldTypeOID:

                                        featureRec_DataRow[feature_IFeature.Fields.Field[i].Name] = Convert.ToInt32(feature_IFeature.Value[i]);
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeGeometry:

                                        featureRec_DataRow[feature_IFeature.Fields.Field[i].Name] = 
                                            
                                            feature_IFeature.Shape.GeometryType.ToString();
                                                                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeInteger:
                                        
                                        featureRec_DataRow[feature_IFeature.Fields.Field[i].Name] = Convert.ToInt32(feature_IFeature.Value[i]);
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeString:

                                        featureRec_DataRow[feature_IFeature.Fields.Field[i].Name] =

                                            feature_IFeature.Value[i].ToString();
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeDouble:

                                        featureRec_DataRow[feature_IFeature.Fields.Field[i].Name] =

                                            Convert.ToDouble(feature_IFeature.Value[i]);
                                        
                                        break;
                                }     
                            }

                            featureTable_DataTable.Rows.Add(featureRec_DataRow);

                            feature_IFeature = cursor_IFeatureCursor.NextFeature();
                        }
                        
                        if(drawshape)
                            
                            axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, Type.Missing, Type.Missing);
                    }
                }
            }
        }
        
        // 查询
        
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
                      
                      // 鼠标拖动创建矩形框
                      
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
        
        // 获取列索引
        
        private int index = 0;
        
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            index = e.ColumnIndex;
        }
        
        // 删除
        
        private void DeleteAttriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fieldName = dataGridView1.Columns[index].Name;

            var featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;

            var featureClass = featureLayer.FeatureClass;

            int n = featureClass.FindField(fieldName);

            if (n != -1)
            {
                ISchemaLock schemaLock = featureClass as ISchemaLock;
                
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                IField field = featureClass.Fields.Field[n];
                
                featureClass.DeleteField(field);
                
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
            }

        }

        // 添加
        
        private void AddAttriToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

       

    }
}
