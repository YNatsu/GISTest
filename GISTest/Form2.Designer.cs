namespace GISTest
{
    partial class LayerAttrib
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerAttrib));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.btModify = new System.Windows.Forms.Button();
            this.btBoxSearch = new System.Windows.Forms.Button();
            this.btAttriSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStringTestValue = new System.Windows.Forms.TextBox();
            this.txtWhereClause = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加测试字段ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除测试字段ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(484, 385);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(502, 14);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(307, 436);
            this.axMapControl1.TabIndex = 1;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            // 
            // btModify
            // 
            this.btModify.Location = new System.Drawing.Point(340, 403);
            this.btModify.Name = "btModify";
            this.btModify.Size = new System.Drawing.Size(75, 23);
            this.btModify.TabIndex = 2;
            this.btModify.Text = "修改";
            this.btModify.UseVisualStyleBackColor = true;
            // 
            // btBoxSearch
            // 
            this.btBoxSearch.Location = new System.Drawing.Point(421, 429);
            this.btBoxSearch.Name = "btBoxSearch";
            this.btBoxSearch.Size = new System.Drawing.Size(75, 23);
            this.btBoxSearch.TabIndex = 3;
            this.btBoxSearch.Text = "框选";
            this.btBoxSearch.UseVisualStyleBackColor = true;
            this.btBoxSearch.Click += new System.EventHandler(this.btBoxSearch_Click);
            // 
            // btAttriSearch
            // 
            this.btAttriSearch.Location = new System.Drawing.Point(340, 429);
            this.btAttriSearch.Name = "btAttriSearch";
            this.btAttriSearch.Size = new System.Drawing.Size(75, 23);
            this.btAttriSearch.TabIndex = 4;
            this.btAttriSearch.Text = "查询";
            this.btAttriSearch.UseVisualStyleBackColor = true;
            this.btAttriSearch.Click += new System.EventHandler(this.btAttriSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 413);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "测试字段值：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 429);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "属性过滤条件：";
            // 
            // txtStringTestValue
            // 
            this.txtStringTestValue.Location = new System.Drawing.Point(99, 405);
            this.txtStringTestValue.Name = "txtStringTestValue";
            this.txtStringTestValue.Size = new System.Drawing.Size(235, 21);
            this.txtStringTestValue.TabIndex = 7;
            // 
            // txtWhereClause
            // 
            this.txtWhereClause.Location = new System.Drawing.Point(99, 429);
            this.txtWhereClause.Name = "txtWhereClause";
            this.txtWhereClause.Size = new System.Drawing.Size(235, 21);
            this.txtWhereClause.TabIndex = 8;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加测试字段ToolStripMenuItem,
            this.删除测试字段ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 48);
            // 
            // 添加测试字段ToolStripMenuItem
            // 
            this.添加测试字段ToolStripMenuItem.Name = "添加测试字段ToolStripMenuItem";
            this.添加测试字段ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.添加测试字段ToolStripMenuItem.Text = "添加测试字段";
            this.添加测试字段ToolStripMenuItem.Click += new System.EventHandler(this.AddAttriToolStripMenuItem_Click);
            // 
            // 删除测试字段ToolStripMenuItem
            // 
            this.删除测试字段ToolStripMenuItem.Name = "删除测试字段ToolStripMenuItem";
            this.删除测试字段ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除测试字段ToolStripMenuItem.Text = "删除测试字段";
            this.删除测试字段ToolStripMenuItem.Click += new System.EventHandler(this.DeleteAttriToolStripMenuItem_Click);
            // 
            // LayerAttrib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 460);
            this.Controls.Add(this.txtWhereClause);
            this.Controls.Add(this.txtStringTestValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btAttriSearch);
            this.Controls.Add(this.btBoxSearch);
            this.Controls.Add(this.btModify);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "LayerAttrib";
            this.Text = "图层属性表";
            this.Load += new System.EventHandler(this.LayerAttrib_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.Button btModify;
        private System.Windows.Forms.Button btBoxSearch;
        private System.Windows.Forms.Button btAttriSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStringTestValue;
        private System.Windows.Forms.TextBox txtWhereClause;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加测试字段ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除测试字段ToolStripMenuItem;
    }
}