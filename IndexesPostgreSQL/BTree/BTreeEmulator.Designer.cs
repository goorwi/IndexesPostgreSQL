namespace IndexesPostgreSQL
{
    partial class BTreeEmulator
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "BTreeEmulator";

            this.drawPanel = new System.Windows.Forms.Panel();
            this.actionPanel = new System.Windows.Forms.Panel();
            this.storyPanel = new System.Windows.Forms.Panel();
            this.storyListBox = new System.Windows.Forms.ListBox();
            this.storyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawPanel
            // 
            this.drawPanel.Location = new System.Drawing.Point(0, 0);
            this.drawPanel.Name = "drawPanel";
            this.drawPanel.Size = new System.Drawing.Size(644, 391);
            this.drawPanel.TabIndex = 0;
            // 
            // actionPanel
            // 
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actionPanel.Location = new System.Drawing.Point(0, 397);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Size = new System.Drawing.Size(1220, 53);
            this.actionPanel.TabIndex = 1;
            // 
            // storyPanel
            // 
            this.storyPanel.Location = new System.Drawing.Point(650, 0);
            this.storyPanel.Name = "storyPanel";
            this.storyPanel.Size = new System.Drawing.Size(150, 391);
            this.storyPanel.TabIndex = 2;
            // 
            // storyListBox
            // 
            this.storyListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.storyListBox.FormattingEnabled = true;
            this.storyListBox.ItemHeight = 20;
            this.storyListBox.Location = new System.Drawing.Point(3, 3);
            this.storyListBox.Name = "storyListBox";
            this.storyListBox.Size = new System.Drawing.Size(144, 384);
            this.storyListBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 450);
            this.Controls.Add(this.storyPanel);
            this.Controls.Add(this.actionPanel);
            this.Controls.Add(this.drawPanel);
            this.Name = "BTreeEmulator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.storyPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel drawPanel;
        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.Panel storyPanel;
        private System.Windows.Forms.ListBox storyListBox;
    }
}