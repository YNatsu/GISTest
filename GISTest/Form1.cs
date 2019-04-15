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
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using Object = System.Object;
using Path = System.IO.Path;


namespace GISTest
{
    public partial class Form1 : Form
    {       
        private ILayer HitLayer;
        
        private IMap HitMap;
        
        public Form1()
        {
            InitializeComponent();
            
            HitLayer = null;
            HitMap = null;
        }

        private void Show(string s)
        {
            MessageBox.Show(s);
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            ((IActiveViewEvents_Event) this.axMapControl1.Map).ItemAdded += this.OnItemAdded;
                                 
        }

        private void axMapControl1_OnMapReplaced(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMapReplacedEvent e)
        {
            this.ViewUpdate();
        }

        private void ViewUpdate()
        {
            // 前面代码省略

            // 当主地图显示控件的地图更换时，鹰眼中的地图也跟随更换

            this.axMapControl2.Map = new MapClass();

            // 添加主地图控件中的所有图层到鹰眼控件中

            for (int i = 1; i <= this.axMapControl1.LayerCount; i++)
            {

                this.axMapControl2.AddLayer(this.axMapControl1.get_Layer(this.axMapControl1.LayerCount - i));

            }

            // 设置 MapControl 显示范围至数据的全局范围

            this.axMapControl2.Extent = this.axMapControl1.FullExtent;

            // 刷新鹰眼控件地图

            this.axMapControl2.Refresh();
        }

        private void OnItemAdded(object o)
        {
            this.ViewUpdate();
        }

        private void axMapControl1_OnExtentUpdated(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            // 得到新范围

            IEnvelope pEnv = (IEnvelope)e.newEnvelope;

            IGraphicsContainer pGra = axMapControl2.Map as IGraphicsContainer;

            IActiveView pAv = pGra as IActiveView;

            // 在绘制前，清除 axMapControl2 中的任何图形元素

            pGra.DeleteAllElements();

            IRectangleElement pRectangleEle = new RectangleElementClass();

            IElement pEle = pRectangleEle as IElement;

            pEle.Geometry = pEnv;

            // 设置鹰眼图中的红线框

            IRgbColor pColor = new RgbColorClass();

            pColor.Red = 255;

            pColor.Green = 0;

            pColor.Blue = 0;

            pColor.Transparency = 255;

            // 产生一个线符号对象

            ILineSymbol pOutline = new SimpleLineSymbolClass();

            pOutline.Width = 2;

            pOutline.Color = pColor;

            // 设置颜色属性

            pColor = new RgbColorClass();

            pColor.Red = 255;

            pColor.Green = 0;

            pColor.Blue = 0;

            pColor.Transparency = 0;

            // 设置填充符号的属性

            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();

            pFillSymbol.Color = pColor;

            pFillSymbol.Outline = pOutline;

            IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;

            pFillShapeEle.Symbol = pFillSymbol;

            pGra.AddElement((IElement)pFillShapeEle, 0);

            // 刷新

            pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void axMapControl2_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if (this.axMapControl2.Map.LayerCount != 0)

            {

                // 按下鼠标左键移动矩形框

                if (e.button == 1)

                {

                    IPoint pPoint = new PointClass();

                    pPoint.PutCoords(e.mapX, e.mapY);

                    IEnvelope pEnvelope = this.axMapControl1.Extent;

                    pEnvelope.CenterAt(pPoint);

                    this.axMapControl1.Extent = pEnvelope;

                    this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

                }

                // 按下鼠标右键绘制矩形框

                else if (e.button == 2)

                {

                    IEnvelope pEnvelop = this.axMapControl2.TrackRectangle();

                    this.axMapControl1.Extent = pEnvelop;

                    this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

                }

            }
        }

        private void axMapControl2_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            // 如果不是左键按下就直接返回

            if (e.button != 1) return;

            IPoint pPoint = new PointClass();

            pPoint.PutCoords(e.mapX, e.mapY);

            this.axMapControl1.CenterAt(pPoint);

            this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            
            if (e.button == 2)
            {
                // 右键
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                ILayer layer = null;
                Object other = null;
                Object index = null;
                
                // HitTest 方法用于按照鼠标指针的位置进行鼠标点击测试
                // 该方法的前两个参数表示鼠标位置的 x 和 y 坐标，
                // 接下来的三个参数是 ref 类型，可以作为返回值，分别表达了：
                // item 参数：点中的项目类型；
                // map 参数：点中的 Map 对象；
                // layer 参数：点中的 Layer 对象。
                // 最后两个参数用的较少，不做详细介绍。
                // 其中利用 item 参数就可以判断点中的是什么项目，
                // 而其后的 map 和 layer 参数按照 item 的结果返回对应类型的接口值
                
                axTOCControl1.HitTest(
                    e.x, e.y, ref item,ref map, ref layer, ref other, ref index
                    );
                
                if (item == esriTOCControlItem.esriTOCControlItemMap)
                {
                    //  如果点中了一个 Map 项目
                    MapMenuStrip.Show(axMapControl1, e.x, e.y);
                    HitMap = map as IMap;
                }
                else if(item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    // 如果点中了一个图层项目
                    
                    LayerMenuStrip.Show(axTOCControl1,e.x, e.y);

                    HitLayer = layer;

                }
            }
        }
        
        // 添加 shape 图层
        
        private void AddShapeLayerItem_Click(object sender, EventArgs e)
        {                      
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Filter =  "shp files (*.shp)|*.shp";

            openFileDialog.FilterIndex = 1;

            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // GetDirectoryName 方法可以解析出目录名
                
                string strWorkspace = Path.GetDirectoryName(openFileDialog.FileName);
                
                // GetFileNameWithoutExtension 方法可以解析出
                
                // 不包含扩展名的文件名称（不包含路径名）
                
                string strLayerName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                
                // ESRI.ArcGIS.Geodatabase.IWorkspaceFactory
                // ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass.ShapefileWorkspaceFactoryClass
                
                IWorkspaceFactory wsFactory = new ShapefileWorkspaceFactory();

                // ESRI.ArcGIS.Geodatabase.IWorkspace
                // 调用 OpenFromFile 通过目录名方法打开 Shapefile 数据库
                
                IWorkspace shpWorkspace = wsFactory.OpenFromFile(strWorkspace, 0);

                if (shpWorkspace != null)
                {
                    // ESRI.ArcGIS.Geodatabase.IEnumDataset
                  
                    IEnumDataset shpDatasets =
                        shpWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                    
                    shpDatasets.Reset();

                    IDataset shpDataset = shpDatasets.Next();

                    while (shpDataset != null)
                    {
                        if (shpDataset.Name == strLayerName)
                        {
                            // ESRI.ArcGIS.Carto.IFeatureLayer
                        
                            IFeatureLayer newLayer = new FeatureLayerClass();
                        
                            newLayer.FeatureClass = shpDataset as IFeatureClass;

                            newLayer.Name = strLayerName;
                           
                            axMapControl1.Map.AddLayer(newLayer);
                        
                            axMapControl2.Map.AddLayer(newLayer);
                        
                            break;
                        }

                        shpDataset = shpDatasets.Next();
                    }
                }
            }
        }
        
        // 打开属性表
        
        private void OpenLayerAttribItem_Click(Object sender, EventArgs e)
        {
            // 创建“打开属性表”对话框的实例
            
            LayerAttrib AttriData = new LayerAttrib();
            
            AttriData.Show();
            
            // 调用 AttriData 自定义 ShowFeatureLayerAttrib 方法
            
            AttriData.ShowFeatureLayerAttrib(HitLayer as IFeatureLayer);

            HitLayer = null;
        }
    }
}
