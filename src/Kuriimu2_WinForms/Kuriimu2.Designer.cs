﻿namespace Kuriimu2_WinForms
{
    partial class Kuriimu2
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFiles = new System.Windows.Forms.TabControl();
            this.tabCloseButtons = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(957, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openFiles
            // 
            this.openFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openFiles.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.openFiles.ImageList = this.tabCloseButtons;
            this.openFiles.Location = new System.Drawing.Point(1, 28);
            this.openFiles.Name = "openFiles";
            this.openFiles.SelectedIndex = 0;
            this.openFiles.Size = new System.Drawing.Size(957, 529);
            this.openFiles.TabIndex = 1;
            this.openFiles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.openFiles_DrawItem);
            this.openFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openFiles_MouseUp);
            // 
            // tabCloseButtons
            // 
            this.tabCloseButtons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.tabCloseButtons.ImageSize = new System.Drawing.Size(16, 16);
            this.tabCloseButtons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Kuriimu2
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 557);
            this.Controls.Add(this.openFiles);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Kuriimu2";
            this.Text = "Kuriimu2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Kuriimu2_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Kuriimu2_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Kuriimu2_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.TabControl openFiles;
        private System.Windows.Forms.ImageList tabCloseButtons;
    }
}

