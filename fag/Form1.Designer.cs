namespace fag
{
    partial class FagMain
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
      this.expression = new System.Windows.Forms.TextBox();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.FlowsList = new System.Windows.Forms.ListView();
      this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Tags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Author = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // expression
      // 
      this.expression.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.expression.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.expression.Location = new System.Drawing.Point(0, 0);
      this.expression.Name = "expression";
      this.expression.Size = new System.Drawing.Size(413, 20);
      this.expression.TabIndex = 0;
      this.expression.Text = "*";
      this.expression.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer1.Location = new System.Drawing.Point(0, 12);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.FlowsList);
      this.splitContainer1.Size = new System.Drawing.Size(413, 308);
      this.splitContainer1.SplitterDistance = 119;
      this.splitContainer1.TabIndex = 1;
      this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
      // 
      // FlowsList
      // 
      this.FlowsList.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
      this.FlowsList.Activation = System.Windows.Forms.ItemActivation.OneClick;
      this.FlowsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.FlowsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.Tags,
            this.Title,
            this.Author,
            this.Date});
      this.FlowsList.Cursor = System.Windows.Forms.Cursors.Hand;
      this.FlowsList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.FlowsList.FullRowSelect = true;
      this.FlowsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.FlowsList.HotTracking = true;
      this.FlowsList.HoverSelection = true;
      this.FlowsList.Location = new System.Drawing.Point(0, 0);
      this.FlowsList.MultiSelect = false;
      this.FlowsList.Name = "FlowsList";
      this.FlowsList.Size = new System.Drawing.Size(290, 308);
      this.FlowsList.TabIndex = 0;
      this.FlowsList.UseCompatibleStateImageBehavior = false;
      this.FlowsList.View = System.Windows.Forms.View.Details;
      this.FlowsList.SelectedIndexChanged += new System.EventHandler(this.DropsList_SelectedIndexChanged);
      // 
      // ID
      // 
      this.ID.Tag = "ID";
      this.ID.Text = "ID";
      this.ID.Width = 25;
      // 
      // Tags
      // 
      this.Tags.Tag = "Tags";
      this.Tags.Text = "Tags";
      // 
      // Title
      // 
      this.Title.Tag = "Title";
      this.Title.Text = "Title";
      // 
      // Author
      // 
      this.Author.Tag = "Author";
      this.Author.Text = "Author";
      // 
      // Date
      // 
      this.Date.Tag = "Date";
      this.Date.Text = "Date";
      // 
      // FagMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(413, 320);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.expression);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "FagMain";
      this.Text = "FAG";
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox expression;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView FlowsList;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader Tags;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ColumnHeader Author;
        private System.Windows.Forms.ColumnHeader Date;


    }
}

