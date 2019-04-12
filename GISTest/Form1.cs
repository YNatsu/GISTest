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
using ESRI.ArcGIS.SystemUI;

namespace GISTest
{
    public partial class Form1 : Form
    {
        //TOCControl控件变量
        
        private ITOCControl2 m_tocControl = null;

        //TOCControl中Map菜单

        private IToolbarMenu m_menuMap = null;

        //TOCControl中图层菜单

        private IToolbarMenu m_menuLayer = null;
        
        public Form1()
        {
            InitializeComponent();
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
    }
}
