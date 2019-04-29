using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Microsoft.VisualBasic;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

namespace GISTest
{
    
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
            
            ShowFeatures(featureLayer,null, true);

        }

        // 添加

        public bool AddField(string fieldName, esriFieldType filedType, int fieldLength)
        {
            var featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
            
            try
            {
                IFields pFields = featureLayer.FeatureClass.Fields;
                
                IFieldEdit pFieldEdit;
 
                pFieldEdit = new FieldClass();
                
                if (fieldName.Length > 5)
                    
                    pFieldEdit.Name_2 = fieldName.Substring(0, 5);
                else
                    pFieldEdit.Name_2 = fieldName;
                
                pFieldEdit.Type_2 = filedType;
                
                pFieldEdit.Editable_2 = true;
                
                pFieldEdit.AliasName_2 = fieldName;
                
                pFieldEdit.Length_2 = fieldLength;
                
                ITable pTable = (ITable)featureLayer;
                              
                pTable.AddField(pFieldEdit);
                
                ShowFeatureLayerAttrib(featureLayer);
                
                return true;
             
            }
 
 
            catch (Exception ex)
            {
                return false;
            }
            
           
        }
     
        
        private void AddAttriToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
            Form3 form3 = new Form3(AddField);   
            
            form3.Show();
        }
        
        // 获得渲染属性名

        private string fieldName;
        
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                DataGridView.HitTestInfo hitTestInfo = dataGridView1.HitTest(e.X, e.Y);

                if (hitTestInfo.Type == DataGridViewHitTestType.ColumnHeader)
                {
                    renderStrip.Show(dataGridView1, e.X, e.Y);

                    fieldName = dataGridView1.Columns[hitTestInfo.ColumnIndex].Name;

                }
                else
                {
                    contextMenuStrip1.Show(dataGridView1, e.X, e.Y);
                }
            }
        }

    
        // 唯一值渲染     
        
        // 先获取该字段上全部要素的属性值，然后再剔除重复值
        
        private string[] GetUniqueValues(IFeatureLayer featureLayer, string fieldName)
        {
            if (featureLayer != null)
            {
                List<string> fieldValues = new List<string>();

                IFeatureClass featureClass = featureLayer.FeatureClass;

                int n = featureClass.FindField(fieldName);

                if (n != -1)
                {
                    IFeatureCursor featureCursor = featureClass.Search(null, false);

                    IFeature feature = featureCursor.NextFeature();

                    while (feature != null)
                    {
                        fieldValues.Add(feature.get_Value(n).ToString());

                        feature = featureCursor.NextFeature();
                    }
                    
                    fieldValues.Sort();

                    for (int i = 1; i < fieldValues.Count; i++)
                    {
                        if (fieldValues[i] == fieldValues[i-1])
                        {
                            fieldValues.RemoveAt(i);

                            i--;
                        }
                    }

                    return fieldValues.ToArray();
                }

               
            }
            
            return null;
        }
        
        private void RenderByUniqueValue(IFeatureLayer featureLayer, string fieldName)
        {
            string[] uniqueValues = GetUniqueValues(featureLayer, fieldName);

            if (uniqueValues != null)
            {
                IUniqueValueRenderer uniqueValueRenderer = new UniqueValueRendererClass();

                uniqueValueRenderer.FieldCount = 1;
                
                uniqueValueRenderer.set_Field(0, fieldName);

                IFeatureClass featureClass = featureLayer.FeatureClass;

                int n = featureClass.FindField(fieldName);

                IField field = featureClass.Fields.Field[n];

                if (field.Type == esriFieldType.esriFieldTypeString)
                {
                    // 如果实际字段值类型是字符串型
                    
                    // 则 UniqueValueRenderer 组件对象的字段类型设置为 true
                    
                    uniqueValueRenderer.set_FieldType(0, true);
                }
                else
                {
                    uniqueValueRenderer.set_FieldType(0, false);
                }
                
                IRandomColorRamp randomColorRamp = new RandomColorRampClass();
                
                randomColorRamp.StartHue = 30;

                randomColorRamp.EndHue = 50;

                randomColorRamp.MinSaturation = 20;

                randomColorRamp.MaxSaturation = 40;

                randomColorRamp.MinValue = 0;

                randomColorRamp.MaxValue = 100;

                randomColorRamp.Size = uniqueValues.Length;

                bool b;
                
                randomColorRamp.CreateRamp(out b);

                if (b )
                {
                    for (int i = 0; i < uniqueValues.Length; i++)
                    {
                        ISymbol symbol = null;
                        
                        if (featureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            symbol = new SimpleFillSymbolClass();

                            (symbol as IFillSymbol).Outline.Width = 1.0;

                            (symbol as IFillSymbol).Color = randomColorRamp.get_Color(i);
                            
                            uniqueValueRenderer.AddValue(uniqueValues[i], uniqueValues[i], symbol);
                            
                            
                        }
                        else if (featureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                        {
                            symbol = new SimpleLineSymbolClass();
                            
                            (symbol as ILineSymbol).Width = 1.0;

                            (symbol as ILineSymbol).Color = randomColorRamp.get_Color(i);
                            
                            uniqueValueRenderer.AddValue(uniqueValues[i], uniqueValues[i], symbol);

                        }
                    }
                }
                IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;

                if (geoFeatureLayer != null)
                {
                    geoFeatureLayer.Renderer = uniqueValueRenderer as IFeatureRenderer;

                    IActiveView activeView = axMapControl1.ActiveView;
                    
                    activeView.ContentsChanged();
                    
                    activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
            }
        }
             
        private void UniqueValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            IFeatureLayer featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
            
            RenderByUniqueValue(featureLayer, fieldName);

        }
        
        // 分级渲染

        private void RenderByClass()
        {
            
        }
        
        private void ClassValueToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("class" );
        }

        

    }
}
