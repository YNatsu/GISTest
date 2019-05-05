using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

namespace GISTest
{
    
    public partial class LayerAttrib : Form
    {
        private int _nSpatialSearchMode;
        
        public LayerAttrib()
        {
            InitializeComponent();

            _nSpatialSearchMode = 0;
        }

        private void LayerAttrib_Load(object sender, EventArgs e)
        {

        }
        
        // 打开属性表时激活

        public void ShowFeatureLayerAttrib(IFeatureLayer layerIFeatureLayer)
        {
            if (layerIFeatureLayer != null)
            {
                axMapControl1.ClearLayers();
                
                axMapControl1.AddLayer(layerIFeatureLayer);
                
                DataTable  featureTableDataTable = new DataTable();
                
                // 矢量要素图层包含的要素类
                
                IFeatureClass fcLayerIFeatureClass = layerIFeatureLayer.FeatureClass;
                
                // 要素类中包含的属性字段总数

                int nFieldCount = fcLayerIFeatureClass.Fields.FieldCount;

                for (int i = 0; i < nFieldCount; i++)
                {
                    DataColumn fieldDataColumn = new DataColumn {ColumnName = fcLayerIFeatureClass.Fields.Field[i].Name};
                    
                    switch (fcLayerIFeatureClass.Fields.Field[i].Type)
                    {
                          case  esriFieldType.esriFieldTypeOID:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                              
                          
                          case esriFieldType.esriFieldTypeGeometry:

                              fieldDataColumn.DataType = Type.GetType("System.String");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeInteger:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeSingle:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeSmallInteger:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeString:
                              
                              fieldDataColumn.DataType = Type.GetType("System.String");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeDouble:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Double");
                              
                              break;
                    }

                    featureTableDataTable.Columns.Add(fieldDataColumn);
                }

                dataGridView1.DataSource = featureTableDataTable;
                
            }

            else
            {
                axMapControl1.ClearLayers();
            }
        }

        private void ShowFeatures(IFeatureLayer featureLayerIFeatureLayer, IQueryFilter conditionIQueryFilter, bool drawshape)
        {
            
            if (featureLayerIFeatureLayer != null)
            {
                // 矢量要素图层关联的要素类
                
                // IFeatureClass => IFeatureLayer.FeatureClass
                
                IFeatureClass featureClassIFeatureClass = featureLayerIFeatureLayer.FeatureClass;
                
                // IFeatureCursor 变量用于获取查询结果
                
                // IFeatureCursor => IFeatureClass.Search(...)
                
                IFeatureCursor cursorIFeatureCursor;

                try
                {
                    cursorIFeatureCursor = featureClassIFeatureClass.Search(conditionIQueryFilter, false);
                }
                catch (Exception)
                {
                    cursorIFeatureCursor = null;
                }

                if (cursorIFeatureCursor != null)
                {
                    axMapControl1.Refresh(
                        esriViewDrawPhase.esriViewGeoSelection, Type.Missing, Type.Missing
                        );
                    
                    // IFeatureSelection =>  IFeatureLayer
                    
                    IFeatureSelection featureSelectionIFeatureSelection = featureLayerIFeatureLayer as IFeatureSelection;

                    if (featureSelectionIFeatureSelection != null)
                    {
                        featureSelectionIFeatureSelection.Clear();

                        DataTable featureTableDataTable = (DataTable) dataGridView1.DataSource;

                        featureTableDataTable.Rows.Clear();

                        IFeature featureIFeature = cursorIFeatureCursor.NextFeature();

                        while (featureIFeature != null)
                        {
                            if (drawshape)
                            {
                                featureSelectionIFeatureSelection.Add(featureIFeature);
                            }

                            DataRow featureRecDataRow = featureTableDataTable.NewRow();

                            int nFieldCount = featureIFeature.Fields.FieldCount;
                            
                            // 依次读取矢量要素的每一个字段值
                            
                            for (int i = 0; i < nFieldCount; i++)
                            {
                                switch (featureIFeature.Fields.Field[i].Type)
                                {
                                    case esriFieldType.esriFieldTypeOID:

                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] = Convert.ToInt32(featureIFeature.Value[i]);
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeGeometry:

                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] = 
                                            
                                            featureIFeature.Shape.GeometryType.ToString();
                                                                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeInteger:
                                        
                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] = Convert.ToInt32(featureIFeature.Value[i]);
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeString:

                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] =

                                            featureIFeature.Value[i].ToString();
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeDouble:

                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] =

                                            Convert.ToDouble(featureIFeature.Value[i]);
                                        
                                        break;
                                }     
                            }

                            featureTableDataTable.Rows.Add(featureRecDataRow);

                            featureIFeature = cursorIFeatureCursor.NextFeature();
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
                _nSpatialSearchMode = 1;
            }
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            switch (_nSpatialSearchMode)
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
        
        private int _index = 0;
        
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            _index = e.ColumnIndex;
        }
        
        // 删除
        
     
        private void DeleteAttriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fieldName = dataGridView1.Columns[_index].Name;

            var featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;

            if (featureLayer != null)
            {

                var featureClass = featureLayer.FeatureClass;

                int n = featureClass.FindField(fieldName);

                if (n != -1)
                {
                    ISchemaLock schemaLock = featureClass as ISchemaLock;

                    if (schemaLock != null)
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                        IField field = featureClass.Fields.Field[n];

                        featureClass.DeleteField(field);

                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    }
                }
                
                
            }

            ShowFeatureLayerAttrib(featureLayer);
            
            ShowFeatures(featureLayer, null, false);
            
        }

        // 添加

        public bool AddField(string fieldName, esriFieldType filedType, int fieldLength)
        {
            var featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
            
            try
            {
                if (featureLayer != null)
                {
                    IFields pFields = featureLayer.FeatureClass.Fields;
                }

                IFieldEdit pFieldEdit = new FieldClass();
                
                pFieldEdit.Name_2 = fieldName.Length > 5 ? fieldName.Substring(0, 5) : fieldName;
                
                pFieldEdit.Type_2 = filedType;
                
                pFieldEdit.Editable_2 = true;
                
                pFieldEdit.AliasName_2 = fieldName;
                
                pFieldEdit.Length_2 = fieldLength;
                
                ITable pTable = (ITable)featureLayer;

                if (pTable != null) pTable.AddField(pFieldEdit);

                ShowFeatureLayerAttrib(featureLayer);
                
                return true;
             
            }
 
 
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            
           
        }
     
        
        private void AddAttriToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
            Form3 form3 = new Form3(AddField);   
            
            form3.Show();
        }
        
        // 获得渲染属性名

        private string _fieldName;
        
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                DataGridView.HitTestInfo hitTestInfo = dataGridView1.HitTest(e.X, e.Y);

                if (hitTestInfo.Type == DataGridViewHitTestType.ColumnHeader)
                {
                    renderStrip.Show(dataGridView1, e.X, e.Y);

                    _fieldName = dataGridView1.Columns[hitTestInfo.ColumnIndex].Name;

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
                        fieldValues.Add(feature.Value[n].ToString());

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
                
                uniqueValueRenderer.Field[0] = fieldName;

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
            
            RenderByUniqueValue(featureLayer, _fieldName);

        }
        
        // 分级渲染

        private void RenderByClass(IFeatureLayer featureLayer, string fieldName, int nBreaks)
        {
            if (featureLayer != null)
            {
                IFeatureClass featureClass = featureLayer.FeatureClass;

                int n = featureClass.FindField(fieldName);

                var type = featureClass.Fields.Field[n].Type;

                if (!(type == esriFieldType.esriFieldTypeDouble || type == esriFieldType.esriFieldTypeInteger))
                {
                    MessageBox.Show("The type of field do not belong to int or double !");
                    
                    return;
                }
                
                if (n != -1 && featureClass.Fields.Field[n].Type == esriFieldType.esriFieldTypeDouble )
                {
                    IClassBreaksRenderer classBreaksRenderer = new ClassBreaksRendererClass();

                    classBreaksRenderer.Field = fieldName;
                    
                    ITableHistogram histogram = new BasicTableHistogramClass();

                    histogram.Field = fieldName;
                    
                    histogram.Table = featureClass as ITable;

                    object dbArray, nArray;

                    (histogram as IBasicHistogram).GetHistogram(out dbArray, out nArray);

                    double[] dataArray = dbArray as double[];

                    int[] freqArray = nArray as int[];
                    
                    // 分级方式
                    
                    IClassifyGEN classifyGen = new EqualIntervalClass();
                    
                    classifyGen.Classify(dataArray, freqArray, nBreaks);

                    classBreaksRenderer.BreakCount = nBreaks;
                    
                    IAlgorithmicColorRamp algorithmicColorRamp = new AlgorithmicColorRampClass();

                    algorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;

                    IRgbColor fromColor = new RgbColorClass();

                    fromColor.Red = 255;
                    
                    fromColor.Green = 255;

                    fromColor.Blue = 0;
                    
                    IRgbColor toColor = new RgbColorClass();

                    toColor.Red = 255;

                    toColor.Green = 0;

                    toColor.Blue = 0;

                    algorithmicColorRamp.FromColor = fromColor;

                    algorithmicColorRamp.ToColor = toColor;
                                      
                    algorithmicColorRamp.Size = nBreaks;

                    bool o;
                    
                    algorithmicColorRamp.CreateRamp(out o);

                    if (o)
                    {
                        double[] breaks = classifyGen.ClassBreaks as double[];

                        if (breaks != null)
                        {
                            classBreaksRenderer.MinimumBreak = breaks[0];

                            for (int i = 0; i < classBreaksRenderer.BreakCount; i++)
                            {
                                classBreaksRenderer.Break[i] = breaks[i];

                                classBreaksRenderer.Label[i] =  breaks[i].ToString(CultureInfo.InvariantCulture) + "-" + breaks[i + 1].ToString();

                                ISymbol symbol;

                                if (featureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                {
                                    symbol = new SimpleFillSymbolClass();

                                    (symbol as IFillSymbol).Outline.Width = 1.0;

                                    (symbol as IFillSymbol).Color = algorithmicColorRamp.Color[i];

                                    classBreaksRenderer.Symbol[i] = symbol;
                                }
                                else if (featureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                {
                                    symbol = new SimpleLineSymbolClass();

                                    (symbol as ILineSymbol).Width = 1.0;

                                    (symbol as ILineSymbol).Color = algorithmicColorRamp.Color[i];

                                    classBreaksRenderer.Symbol[i] = symbol;
                                }
                                else
                                {
                                    MessageBox.Show("The ShapeType of this do not belong to Polygon or Polyline");
                                }
                            }
                        }
                    }
                    
                    // 6.4
                    
                    IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;

                    if (geoFeatureLayer != null)
                    {
                        geoFeatureLayer.Renderer = classBreaksRenderer as IFeatureRenderer;

                        IActiveView activeView = axMapControl1.ActiveView;
                        
                        activeView.ContentsChanged();
                        
                        activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                    }
                }
            }
        }

        private void RenderClass(int n)
        {
            IFeatureLayer featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
            
            RenderByClass(featureLayer, _fieldName, n);
        }
        
        private void ClassValueToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
        
            Form4 form4 = new Form4(RenderClass);
          
            form4.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _nSpatialSearchMode = 0;
        }


    }
}
