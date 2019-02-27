namespace 老数据导入
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.grid = new System.Windows.Forms.DataGridView();
            this.b获取数据 = new System.Windows.Forms.Button();
            this.b开始导入 = new System.Windows.Forms.Button();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.申请人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.导入错误原因 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.激励错误 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预算收入错误 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预算支出错误 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.实际收入错误 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.实际支出错误 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预算编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预算名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.销售 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预算说明 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.成本中心编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.业务类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否中证通 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.子订单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.线下线上 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.审批结果 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.可用否 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.更新时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预算状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.申请日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.公证处 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.激励预算编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.激励预算值百分比 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.激励值 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.备用 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.b开始导入中证通 = new System.Windows.Forms.Button();
            this.b获取数据中证通 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToOrderColumns = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.ID,
            this.申请人,
            this.导入错误原因,
            this.激励错误,
            this.预算收入错误,
            this.预算支出错误,
            this.实际收入错误,
            this.实际支出错误,
            this.预算编号,
            this.预算名称,
            this.类型,
            this.销售,
            this.预算说明,
            this.成本中心编号,
            this.业务类型,
            this.是否中证通,
            this.子订单号,
            this.线下线上,
            this.审批结果,
            this.可用否,
            this.更新时间,
            this.预算状态,
            this.申请日期,
            this.公证处,
            this.激励预算编号,
            this.激励预算值百分比,
            this.激励值,
            this.备用});
            this.grid.Location = new System.Drawing.Point(12, 12);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.Height = 23;
            this.grid.Size = new System.Drawing.Size(1096, 485);
            this.grid.TabIndex = 4;
            // 
            // b获取数据
            // 
            this.b获取数据.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b获取数据.Location = new System.Drawing.Point(112, 515);
            this.b获取数据.Name = "b获取数据";
            this.b获取数据.Size = new System.Drawing.Size(110, 34);
            this.b获取数据.TabIndex = 5;
            this.b获取数据.Text = "获取数据";
            this.b获取数据.UseVisualStyleBackColor = true;
            this.b获取数据.Click += new System.EventHandler(this.b获取数据_Click);
            // 
            // b开始导入
            // 
            this.b开始导入.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b开始导入.Location = new System.Drawing.Point(247, 515);
            this.b开始导入.Name = "b开始导入";
            this.b开始导入.Size = new System.Drawing.Size(110, 34);
            this.b开始导入.TabIndex = 6;
            this.b开始导入.Text = "开始导入";
            this.b开始导入.UseVisualStyleBackColor = true;
            this.b开始导入.Click += new System.EventHandler(this.b开始导入_Click);
            // 
            // 序号
            // 
            this.序号.DataPropertyName = "序号";
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            this.序号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.序号.Width = 50;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ID.Width = 30;
            // 
            // 申请人
            // 
            this.申请人.DataPropertyName = "申请人";
            this.申请人.HeaderText = "申请人";
            this.申请人.Name = "申请人";
            this.申请人.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 导入错误原因
            // 
            this.导入错误原因.DataPropertyName = "导入错误原因";
            this.导入错误原因.HeaderText = "导入错误原因";
            this.导入错误原因.Name = "导入错误原因";
            // 
            // 激励错误
            // 
            this.激励错误.DataPropertyName = "激励错误";
            this.激励错误.HeaderText = "激励错误";
            this.激励错误.Name = "激励错误";
            // 
            // 预算收入错误
            // 
            this.预算收入错误.DataPropertyName = "预算收入错误";
            this.预算收入错误.HeaderText = "预算收入错误";
            this.预算收入错误.Name = "预算收入错误";
            // 
            // 预算支出错误
            // 
            this.预算支出错误.DataPropertyName = "预算支出错误";
            this.预算支出错误.HeaderText = "预算支出错误";
            this.预算支出错误.Name = "预算支出错误";
            // 
            // 实际收入错误
            // 
            this.实际收入错误.DataPropertyName = "实际收入错误";
            this.实际收入错误.HeaderText = "实际收入错误";
            this.实际收入错误.Name = "实际收入错误";
            // 
            // 实际支出错误
            // 
            this.实际支出错误.DataPropertyName = "实际支出错误";
            this.实际支出错误.HeaderText = "实际支出错误";
            this.实际支出错误.Name = "实际支出错误";
            // 
            // 预算编号
            // 
            this.预算编号.DataPropertyName = "预算编号";
            this.预算编号.HeaderText = "预算编号";
            this.预算编号.Name = "预算编号";
            this.预算编号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 预算名称
            // 
            this.预算名称.DataPropertyName = "预算名称";
            this.预算名称.HeaderText = "预算名称";
            this.预算名称.Name = "预算名称";
            this.预算名称.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 类型
            // 
            this.类型.DataPropertyName = "类型";
            this.类型.HeaderText = "类型";
            this.类型.Name = "类型";
            this.类型.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 销售
            // 
            this.销售.DataPropertyName = "销售";
            this.销售.HeaderText = "销售";
            this.销售.Name = "销售";
            this.销售.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 预算说明
            // 
            this.预算说明.DataPropertyName = "预算说明";
            this.预算说明.HeaderText = "预算说明";
            this.预算说明.Name = "预算说明";
            this.预算说明.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 成本中心编号
            // 
            this.成本中心编号.DataPropertyName = "成本中心编号";
            this.成本中心编号.HeaderText = "成本中心编号";
            this.成本中心编号.Name = "成本中心编号";
            this.成本中心编号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 业务类型
            // 
            this.业务类型.DataPropertyName = "业务类型";
            this.业务类型.HeaderText = "业务类型";
            this.业务类型.Name = "业务类型";
            this.业务类型.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 是否中证通
            // 
            this.是否中证通.DataPropertyName = "是否中证通";
            this.是否中证通.HeaderText = "是否中证通";
            this.是否中证通.Name = "是否中证通";
            this.是否中证通.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 子订单号
            // 
            this.子订单号.DataPropertyName = "子订单号";
            this.子订单号.HeaderText = "子订单号";
            this.子订单号.Name = "子订单号";
            this.子订单号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 线下线上
            // 
            this.线下线上.DataPropertyName = "线下线上";
            this.线下线上.HeaderText = "线下线上";
            this.线下线上.Name = "线下线上";
            this.线下线上.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 审批结果
            // 
            this.审批结果.DataPropertyName = "审批结果";
            this.审批结果.HeaderText = "审批结果";
            this.审批结果.Name = "审批结果";
            this.审批结果.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 可用否
            // 
            this.可用否.DataPropertyName = "可用否";
            this.可用否.HeaderText = "可用否";
            this.可用否.Name = "可用否";
            this.可用否.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 更新时间
            // 
            this.更新时间.DataPropertyName = "更新时间";
            this.更新时间.HeaderText = "更新时间";
            this.更新时间.Name = "更新时间";
            this.更新时间.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 预算状态
            // 
            this.预算状态.DataPropertyName = "预算状态";
            this.预算状态.HeaderText = "预算状态";
            this.预算状态.Name = "预算状态";
            this.预算状态.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 申请日期
            // 
            this.申请日期.DataPropertyName = "申请日期";
            this.申请日期.HeaderText = "申请日期";
            this.申请日期.Name = "申请日期";
            this.申请日期.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 公证处
            // 
            this.公证处.DataPropertyName = "公证处";
            this.公证处.HeaderText = "公证处";
            this.公证处.Name = "公证处";
            this.公证处.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 激励预算编号
            // 
            this.激励预算编号.DataPropertyName = "激励预算编号";
            this.激励预算编号.HeaderText = "激励预算编号";
            this.激励预算编号.Name = "激励预算编号";
            this.激励预算编号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 激励预算值百分比
            // 
            this.激励预算值百分比.DataPropertyName = "激励预算值百分比";
            this.激励预算值百分比.HeaderText = "激励预算值百分比";
            this.激励预算值百分比.Name = "激励预算值百分比";
            this.激励预算值百分比.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 激励值
            // 
            this.激励值.DataPropertyName = "激励值";
            this.激励值.HeaderText = "激励值";
            this.激励值.Name = "激励值";
            this.激励值.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 备用
            // 
            this.备用.DataPropertyName = "备用";
            this.备用.HeaderText = "备用";
            this.备用.Name = "备用";
            this.备用.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // b开始导入中证通
            // 
            this.b开始导入中证通.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b开始导入中证通.Location = new System.Drawing.Point(863, 515);
            this.b开始导入中证通.Name = "b开始导入中证通";
            this.b开始导入中证通.Size = new System.Drawing.Size(110, 34);
            this.b开始导入中证通.TabIndex = 8;
            this.b开始导入中证通.Text = "开始导入中证通";
            this.b开始导入中证通.UseVisualStyleBackColor = true;
            this.b开始导入中证通.Click += new System.EventHandler(this.b开始导入中证通_Click);
            // 
            // b获取数据中证通
            // 
            this.b获取数据中证通.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b获取数据中证通.Location = new System.Drawing.Point(728, 515);
            this.b获取数据中证通.Name = "b获取数据中证通";
            this.b获取数据中证通.Size = new System.Drawing.Size(110, 34);
            this.b获取数据中证通.TabIndex = 7;
            this.b获取数据中证通.Text = "获取数据中证通";
            this.b获取数据中证通.UseVisualStyleBackColor = true;
            this.b获取数据中证通.Click += new System.EventHandler(this.b获取数据中证通_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 561);
            this.Controls.Add(this.b开始导入中证通);
            this.Controls.Add(this.b获取数据中证通);
            this.Controls.Add(this.b开始导入);
            this.Controls.Add(this.b获取数据);
            this.Controls.Add(this.grid);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button b获取数据;
        private System.Windows.Forms.Button b开始导入;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 申请人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 导入错误原因;
        private System.Windows.Forms.DataGridViewTextBoxColumn 激励错误;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预算收入错误;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预算支出错误;
        private System.Windows.Forms.DataGridViewTextBoxColumn 实际收入错误;
        private System.Windows.Forms.DataGridViewTextBoxColumn 实际支出错误;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预算编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预算名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 销售;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预算说明;
        private System.Windows.Forms.DataGridViewTextBoxColumn 成本中心编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 业务类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 是否中证通;
        private System.Windows.Forms.DataGridViewTextBoxColumn 子订单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 线下线上;
        private System.Windows.Forms.DataGridViewTextBoxColumn 审批结果;
        private System.Windows.Forms.DataGridViewTextBoxColumn 可用否;
        private System.Windows.Forms.DataGridViewTextBoxColumn 更新时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预算状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn 申请日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 公证处;
        private System.Windows.Forms.DataGridViewTextBoxColumn 激励预算编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 激励预算值百分比;
        private System.Windows.Forms.DataGridViewTextBoxColumn 激励值;
        private System.Windows.Forms.DataGridViewTextBoxColumn 备用;
        private System.Windows.Forms.Button b开始导入中证通;
        private System.Windows.Forms.Button b获取数据中证通;
    }
}

