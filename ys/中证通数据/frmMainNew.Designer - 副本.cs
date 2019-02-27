namespace 中证通数据
{
    partial class frmMainNew
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainNew));
            this.b立即获取 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.b设为未导入 = new System.Windows.Forms.Button();
            this.b停止 = new System.Windows.Forms.Button();
            this.b仅获取 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.t1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.grid = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.当事人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.导入失败原因 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.当事人证件号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.子订单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.承办公证处 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.公证事项 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.用地 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.语种 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否加急 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.销售 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.公证费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.加急费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.证词翻译费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.证词翻译费_支出 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.证词翻译费_支出方 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.附件翻译费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.附件翻译费_支出 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.附件翻译费_支出方 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.官方认证费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.外办认证费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.服务认证费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.副本费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.渠道服务费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.人科服务费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.调查费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.翻译服务费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.优惠金额_支出 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.退回费用_支出 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.实际收入 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.认证费成本 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.渠道 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.业务类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.线上线下 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.子订单ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // b立即获取
            // 
            this.b立即获取.Location = new System.Drawing.Point(615, 14);
            this.b立即获取.Name = "b立即获取";
            this.b立即获取.Size = new System.Drawing.Size(117, 30);
            this.b立即获取.TabIndex = 0;
            this.b立即获取.Text = "立即获取并导入";
            this.b立即获取.UseVisualStyleBackColor = true;
            this.b立即获取.Click += new System.EventHandler(this.b立即获取_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.b设为未导入);
            this.panel1.Controls.Add(this.b停止);
            this.panel1.Controls.Add(this.b仅获取);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lbl2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lbl1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.t1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.b立即获取);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1226, 59);
            this.panel1.TabIndex = 1;
            // 
            // b设为未导入
            // 
            this.b设为未导入.Location = new System.Drawing.Point(1025, 14);
            this.b设为未导入.Name = "b设为未导入";
            this.b设为未导入.Size = new System.Drawing.Size(177, 30);
            this.b设为未导入.TabIndex = 24;
            this.b设为未导入.Text = "设置中证通全部数据为未导入";
            this.b设为未导入.UseVisualStyleBackColor = true;
            this.b设为未导入.Click += new System.EventHandler(this.b设为未导入_Click);
            // 
            // b停止
            // 
            this.b停止.Enabled = false;
            this.b停止.Location = new System.Drawing.Point(738, 14);
            this.b停止.Name = "b停止";
            this.b停止.Size = new System.Drawing.Size(43, 30);
            this.b停止.TabIndex = 23;
            this.b停止.Text = "停止";
            this.b停止.UseVisualStyleBackColor = true;
            this.b停止.Click += new System.EventHandler(this.b停止_Click);
            // 
            // b仅获取
            // 
            this.b仅获取.Location = new System.Drawing.Point(842, 14);
            this.b仅获取.Name = "b仅获取";
            this.b仅获取.Size = new System.Drawing.Size(177, 30);
            this.b仅获取.TabIndex = 22;
            this.b仅获取.Text = "仅显示要导入数据（检测用）";
            this.b仅获取.UseVisualStyleBackColor = true;
            this.b仅获取.Click += new System.EventHandler(this.b仅获取_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(542, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "导入失败";
            // 
            // lbl2
            // 
            this.lbl2.BackColor = System.Drawing.Color.Coral;
            this.lbl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl2.Location = new System.Drawing.Point(519, 21);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(16, 16);
            this.lbl2.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(447, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "导入成功";
            // 
            // lbl1
            // 
            this.lbl1.BackColor = System.Drawing.Color.Yellow;
            this.lbl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl1.Location = new System.Drawing.Point(424, 21);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(16, 16);
            this.lbl1.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "分钟（5～1440）";
            // 
            // t1
            // 
            this.t1.Location = new System.Drawing.Point(249, 20);
            this.t1.Name = "t1";
            this.t1.Size = new System.Drawing.Size(43, 21);
            this.t1.TabIndex = 1;
            this.t1.Text = "10";
            this.t1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.t1.TextChanged += new System.EventHandler(this.t1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "预算系统自动获取中证通数据时间周期";
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToOrderColumns = true;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.当事人,
            this.导入失败原因,
            this.当事人证件号,
            this.订单号,
            this.订单日期,
            this.子订单号,
            this.承办公证处,
            this.公证事项,
            this.用地,
            this.语种,
            this.是否加急,
            this.销售,
            this.公证费,
            this.加急费,
            this.证词翻译费,
            this.证词翻译费_支出,
            this.证词翻译费_支出方,
            this.附件翻译费,
            this.附件翻译费_支出,
            this.附件翻译费_支出方,
            this.官方认证费,
            this.外办认证费,
            this.服务认证费,
            this.副本费,
            this.渠道服务费,
            this.人科服务费,
            this.调查费,
            this.翻译服务费,
            this.优惠金额_支出,
            this.退回费用_支出,
            this.实际收入,
            this.认证费成本,
            this.渠道,
            this.业务类型,
            this.线上线下,
            this.订单ID,
            this.子订单ID});
            this.grid.Location = new System.Drawing.Point(95, 118);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.Height = 23;
            this.grid.Size = new System.Drawing.Size(827, 142);
            this.grid.TabIndex = 3;
            // 
            // 序号
            // 
            this.序号.DataPropertyName = "序号";
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            this.序号.Width = 50;
            // 
            // 当事人
            // 
            this.当事人.DataPropertyName = "当事人";
            this.当事人.HeaderText = "当事人";
            this.当事人.Name = "当事人";
            // 
            // 导入失败原因
            // 
            this.导入失败原因.DataPropertyName = "导入失败原因";
            this.导入失败原因.HeaderText = "导入失败原因";
            this.导入失败原因.Name = "导入失败原因";
            // 
            // 当事人证件号
            // 
            this.当事人证件号.DataPropertyName = "当事人证件号";
            this.当事人证件号.HeaderText = "当事人证件号";
            this.当事人证件号.Name = "当事人证件号";
            // 
            // 订单号
            // 
            this.订单号.DataPropertyName = "订单号";
            this.订单号.HeaderText = "订单号";
            this.订单号.Name = "订单号";
            // 
            // 订单日期
            // 
            this.订单日期.DataPropertyName = "订单日期";
            this.订单日期.HeaderText = "订单日期";
            this.订单日期.Name = "订单日期";
            // 
            // 子订单号
            // 
            this.子订单号.DataPropertyName = "子订单号";
            this.子订单号.HeaderText = "子订单号";
            this.子订单号.Name = "子订单号";
            // 
            // 承办公证处
            // 
            this.承办公证处.DataPropertyName = "承办公证处";
            this.承办公证处.HeaderText = "承办公证处";
            this.承办公证处.Name = "承办公证处";
            // 
            // 公证事项
            // 
            this.公证事项.DataPropertyName = "公证事项";
            this.公证事项.HeaderText = "公证事项";
            this.公证事项.Name = "公证事项";
            // 
            // 用地
            // 
            this.用地.DataPropertyName = "用地";
            this.用地.HeaderText = "用地";
            this.用地.Name = "用地";
            // 
            // 语种
            // 
            this.语种.DataPropertyName = "语种";
            this.语种.HeaderText = "语种";
            this.语种.Name = "语种";
            // 
            // 是否加急
            // 
            this.是否加急.DataPropertyName = "是否加急";
            this.是否加急.HeaderText = "是否加急";
            this.是否加急.Name = "是否加急";
            // 
            // 销售
            // 
            this.销售.DataPropertyName = "销售";
            this.销售.HeaderText = "销售";
            this.销售.Name = "销售";
            // 
            // 公证费
            // 
            this.公证费.DataPropertyName = "公证费";
            this.公证费.HeaderText = "公证费";
            this.公证费.Name = "公证费";
            // 
            // 加急费
            // 
            this.加急费.DataPropertyName = "加急费";
            this.加急费.HeaderText = "加急费";
            this.加急费.Name = "加急费";
            // 
            // 证词翻译费
            // 
            this.证词翻译费.DataPropertyName = "证词翻译费";
            this.证词翻译费.HeaderText = "证词翻译费";
            this.证词翻译费.Name = "证词翻译费";
            // 
            // 证词翻译费_支出
            // 
            this.证词翻译费_支出.DataPropertyName = "证词翻译费_支出";
            this.证词翻译费_支出.HeaderText = "证词翻译费_支出";
            this.证词翻译费_支出.Name = "证词翻译费_支出";
            // 
            // 证词翻译费_支出方
            // 
            this.证词翻译费_支出方.DataPropertyName = "证词翻译费_支出方";
            this.证词翻译费_支出方.HeaderText = "证词翻译费_支出方";
            this.证词翻译费_支出方.Name = "证词翻译费_支出方";
            // 
            // 附件翻译费
            // 
            this.附件翻译费.DataPropertyName = "附件翻译费";
            this.附件翻译费.HeaderText = "附件翻译费";
            this.附件翻译费.Name = "附件翻译费";
            // 
            // 附件翻译费_支出
            // 
            this.附件翻译费_支出.DataPropertyName = "附件翻译费_支出";
            this.附件翻译费_支出.HeaderText = "附件翻译费_支出";
            this.附件翻译费_支出.Name = "附件翻译费_支出";
            // 
            // 附件翻译费_支出方
            // 
            this.附件翻译费_支出方.DataPropertyName = "附件翻译费_支出方";
            this.附件翻译费_支出方.HeaderText = "附件翻译费_支出方";
            this.附件翻译费_支出方.Name = "附件翻译费_支出方";
            // 
            // 官方认证费
            // 
            this.官方认证费.DataPropertyName = "官方认证费";
            this.官方认证费.HeaderText = "官方认证费";
            this.官方认证费.Name = "官方认证费";
            // 
            // 外办认证费
            // 
            this.外办认证费.DataPropertyName = "外办认证费";
            this.外办认证费.HeaderText = "外办认证费";
            this.外办认证费.Name = "外办认证费";
            // 
            // 服务认证费
            // 
            this.服务认证费.DataPropertyName = "服务认证费";
            this.服务认证费.HeaderText = "服务认证费";
            this.服务认证费.Name = "服务认证费";
            // 
            // 副本费
            // 
            this.副本费.DataPropertyName = "副本费";
            this.副本费.HeaderText = "副本费";
            this.副本费.Name = "副本费";
            // 
            // 渠道服务费
            // 
            this.渠道服务费.DataPropertyName = "渠道服务费";
            this.渠道服务费.HeaderText = "渠道服务费";
            this.渠道服务费.Name = "渠道服务费";
            // 
            // 人科服务费
            // 
            this.人科服务费.DataPropertyName = "人科服务费";
            this.人科服务费.HeaderText = "人科服务费";
            this.人科服务费.Name = "人科服务费";
            // 
            // 调查费
            // 
            this.调查费.DataPropertyName = "调查费";
            this.调查费.HeaderText = "调查费";
            this.调查费.Name = "调查费";
            // 
            // 翻译服务费
            // 
            this.翻译服务费.DataPropertyName = "翻译服务费";
            this.翻译服务费.HeaderText = "翻译服务费";
            this.翻译服务费.Name = "翻译服务费";
            // 
            // 优惠金额_支出
            // 
            this.优惠金额_支出.DataPropertyName = "优惠金额_支出";
            this.优惠金额_支出.HeaderText = "优惠金额_支出";
            this.优惠金额_支出.Name = "优惠金额_支出";
            // 
            // 退回费用_支出
            // 
            this.退回费用_支出.DataPropertyName = "退回费用_支出";
            this.退回费用_支出.HeaderText = "退回费用_支出";
            this.退回费用_支出.Name = "退回费用_支出";
            // 
            // 实际收入
            // 
            this.实际收入.DataPropertyName = "实际收入";
            this.实际收入.HeaderText = "实际收入";
            this.实际收入.Name = "实际收入";
            // 
            // 认证费成本
            // 
            this.认证费成本.DataPropertyName = "认证费成本";
            this.认证费成本.HeaderText = "认证费成本";
            this.认证费成本.Name = "认证费成本";
            // 
            // 渠道
            // 
            this.渠道.DataPropertyName = "渠道";
            this.渠道.HeaderText = "渠道";
            this.渠道.Name = "渠道";
            // 
            // 业务类型
            // 
            this.业务类型.DataPropertyName = "业务类型";
            this.业务类型.HeaderText = "业务类型";
            this.业务类型.Name = "业务类型";
            // 
            // 线上线下
            // 
            this.线上线下.DataPropertyName = "线上线下";
            this.线上线下.HeaderText = "线上线下";
            this.线上线下.Name = "线上线下";
            // 
            // 订单ID
            // 
            this.订单ID.DataPropertyName = "订单ID";
            this.订单ID.HeaderText = "订单ID";
            this.订单ID.Name = "订单ID";
            this.订单ID.Width = 5;
            // 
            // 子订单ID
            // 
            this.子订单ID.DataPropertyName = "子订单ID";
            this.子订单ID.HeaderText = "子订单ID";
            this.子订单ID.Name = "子订单ID";
            this.子订单ID.Width = 5;
            // 
            // frmMainNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1226, 543);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMainNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中证通数据导入到预算系统";
            this.Load += new System.EventHandler(this.frmMainNew_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button b立即获取;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox t1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Button b仅获取;
        private System.Windows.Forms.Button b停止;
        private System.Windows.Forms.Button b设为未导入;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 当事人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 导入失败原因;
        private System.Windows.Forms.DataGridViewTextBoxColumn 当事人证件号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 子订单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 承办公证处;
        private System.Windows.Forms.DataGridViewTextBoxColumn 公证事项;
        private System.Windows.Forms.DataGridViewTextBoxColumn 用地;
        private System.Windows.Forms.DataGridViewTextBoxColumn 语种;
        private System.Windows.Forms.DataGridViewTextBoxColumn 是否加急;
        private System.Windows.Forms.DataGridViewTextBoxColumn 销售;
        private System.Windows.Forms.DataGridViewTextBoxColumn 公证费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 加急费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证词翻译费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证词翻译费_支出;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证词翻译费_支出方;
        private System.Windows.Forms.DataGridViewTextBoxColumn 附件翻译费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 附件翻译费_支出;
        private System.Windows.Forms.DataGridViewTextBoxColumn 附件翻译费_支出方;
        private System.Windows.Forms.DataGridViewTextBoxColumn 官方认证费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 外办认证费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 服务认证费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 副本费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 渠道服务费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 人科服务费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 调查费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 翻译服务费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 优惠金额_支出;
        private System.Windows.Forms.DataGridViewTextBoxColumn 退回费用_支出;
        private System.Windows.Forms.DataGridViewTextBoxColumn 实际收入;
        private System.Windows.Forms.DataGridViewTextBoxColumn 认证费成本;
        private System.Windows.Forms.DataGridViewTextBoxColumn 渠道;
        private System.Windows.Forms.DataGridViewTextBoxColumn 业务类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 线上线下;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 子订单ID;
    }
}

