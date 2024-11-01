namespace PnT.SongClient.UI.Controls
{
    partial class RegisterBaseControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterBaseControl));
            this.tlpLeft = new System.Windows.Forms.TableLayoutPanel();
            this.lsItems = new System.Windows.Forms.ListBox();
            this.mlblItemHeading = new MetroFramework.Controls.MetroLabel();
            this.pnlLeftTop = new System.Windows.Forms.Panel();
            this.txSearchString = new MetroFramework.Controls.MetroTextBox();
            this.ckHideNotActive = new MetroFramework.Controls.MetroCheckBox();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btCancel = new MetroFramework.Controls.MetroButton();
            this.btSave = new MetroFramework.Controls.MetroButton();
            this.btDelete = new MetroFramework.Controls.MetroButton();
            this.btCopy = new MetroFramework.Controls.MetroButton();
            this.btNew = new MetroFramework.Controls.MetroButton();
            this.btEdit = new MetroFramework.Controls.MetroButton();
            this.tlpHeader = new System.Windows.Forms.TableLayoutPanel();
            this.mtlReturn = new MetroFramework.Controls.MetroTile();
            this.mbtnNextSearchResult = new MetroFramework.Controls.MetroButton();
            this.tlpLeft.SuspendLayout();
            this.pnlLeftTop.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            this.tlpHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLeft
            // 
            this.tlpLeft.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tlpLeft, "tlpLeft");
            this.tlpLeft.Controls.Add(this.lsItems, 0, 2);
            this.tlpLeft.Controls.Add(this.mlblItemHeading, 0, 0);
            this.tlpLeft.Controls.Add(this.pnlLeftTop, 0, 1);
            this.tlpLeft.Controls.Add(this.ckHideNotActive, 0, 3);
            this.tlpLeft.Name = "tlpLeft";
            // 
            // lsItems
            // 
            this.lsItems.CausesValidation = false;
            this.lsItems.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lsItems.DisplayMember = "Description";
            resources.ApplyResources(this.lsItems, "lsItems");
            this.lsItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lsItems.FormattingEnabled = true;
            this.lsItems.Name = "lsItems";
            this.lsItems.ValueMember = "Id";
            this.lsItems.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lsItems_DrawItem);
            this.lsItems.SelectedIndexChanged += new System.EventHandler(this.lsItems_SelectedIndexChanged);
            // 
            // mlblItemHeading
            // 
            resources.ApplyResources(this.mlblItemHeading, "mlblItemHeading");
            this.mlblItemHeading.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.mlblItemHeading.Name = "mlblItemHeading";
            // 
            // pnlLeftTop
            // 
            this.pnlLeftTop.Controls.Add(this.mbtnNextSearchResult);
            this.pnlLeftTop.Controls.Add(this.txSearchString);
            resources.ApplyResources(this.pnlLeftTop, "pnlLeftTop");
            this.pnlLeftTop.Name = "pnlLeftTop";
            // 
            // txSearchString
            // 
            // 
            // 
            // 
            this.txSearchString.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.txSearchString.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.txSearchString.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.txSearchString.CustomButton.Name = "";
            this.txSearchString.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.txSearchString.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txSearchString.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.txSearchString.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txSearchString.CustomButton.UseSelectable = true;
            this.txSearchString.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.txSearchString.Lines = new string[0];
            resources.ApplyResources(this.txSearchString, "txSearchString");
            this.txSearchString.MaxLength = 32767;
            this.txSearchString.Name = "txSearchString";
            this.txSearchString.PasswordChar = '\0';
            this.txSearchString.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txSearchString.SelectedText = "";
            this.txSearchString.SelectionLength = 0;
            this.txSearchString.SelectionStart = 0;
            this.txSearchString.ShortcutsEnabled = true;
            this.txSearchString.UseSelectable = true;
            this.txSearchString.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txSearchString.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txSearchString.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txSearchString_KeyUp);
            // 
            // ckHideNotActive
            // 
            resources.ApplyResources(this.ckHideNotActive, "ckHideNotActive");
            this.ckHideNotActive.Checked = true;
            this.ckHideNotActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckHideNotActive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckHideNotActive.Name = "ckHideNotActive";
            this.ckHideNotActive.UseSelectable = true;
            this.ckHideNotActive.CheckedChanged += new System.EventHandler(this.ckHideNotActive_CheckedChanged);
            // 
            // tlpBottom
            // 
            this.tlpBottom.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tlpBottom, "tlpBottom");
            this.tlpBottom.Controls.Add(this.btCancel, 6, 0);
            this.tlpBottom.Controls.Add(this.btSave, 5, 0);
            this.tlpBottom.Controls.Add(this.btDelete, 4, 0);
            this.tlpBottom.Controls.Add(this.btCopy, 3, 0);
            this.tlpBottom.Controls.Add(this.btNew, 1, 0);
            this.tlpBottom.Controls.Add(this.btEdit, 2, 0);
            this.tlpBottom.Name = "tlpBottom";
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btCancel.Name = "btCancel";
            this.btCancel.UseSelectable = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btSave
            // 
            resources.ApplyResources(this.btSave, "btSave");
            this.btSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btSave.Name = "btSave";
            this.btSave.UseSelectable = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btDelete
            // 
            resources.ApplyResources(this.btDelete, "btDelete");
            this.btDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btDelete.Name = "btDelete";
            this.btDelete.UseSelectable = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btCopy
            // 
            resources.ApplyResources(this.btCopy, "btCopy");
            this.btCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btCopy.Name = "btCopy";
            this.btCopy.UseSelectable = true;
            this.btCopy.Click += new System.EventHandler(this.btCopy_Click);
            // 
            // btNew
            // 
            resources.ApplyResources(this.btNew, "btNew");
            this.btNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btNew.Name = "btNew";
            this.btNew.UseSelectable = true;
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // btEdit
            // 
            resources.ApplyResources(this.btEdit, "btEdit");
            this.btEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btEdit.Name = "btEdit";
            this.btEdit.UseSelectable = true;
            this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // tlpHeader
            // 
            this.tlpHeader.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tlpHeader, "tlpHeader");
            this.tlpHeader.Controls.Add(this.mtlReturn, 1, 0);
            this.tlpHeader.Name = "tlpHeader";
            // 
            // mtlReturn
            // 
            this.mtlReturn.ActiveControl = null;
            this.mtlReturn.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.mtlReturn, "mtlReturn");
            this.mtlReturn.Name = "mtlReturn";
            this.mtlReturn.Style = MetroFramework.MetroColorStyle.White;
            this.mtlReturn.Tag = "Edit";
            this.mtlReturn.TileImage = global::PnT.SongClient.Properties.Resources.IconBackWhite;
            this.mtlReturn.TileImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mtlReturn.UseCustomForeColor = true;
            this.mtlReturn.UseSelectable = true;
            this.mtlReturn.UseTileImage = true;
            this.mtlReturn.Click += new System.EventHandler(this.mtlReturn_Click);
            // 
            // mbtnNextSearchResult
            // 
            this.mbtnNextSearchResult.BackgroundImage = global::PnT.SongClient.Properties.Resources.IconSendDRightOne;
            resources.ApplyResources(this.mbtnNextSearchResult, "mbtnNextSearchResult");
            this.mbtnNextSearchResult.Name = "mbtnNextSearchResult";
            this.mbtnNextSearchResult.UseCustomBackColor = true;
            this.mbtnNextSearchResult.UseSelectable = true;
            this.mbtnNextSearchResult.Click += new System.EventHandler(this.mbtnNextSearchResult_Click);
            // 
            // RegisterBaseControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpHeader);
            this.Controls.Add(this.tlpBottom);
            this.Controls.Add(this.tlpLeft);
            this.Name = "RegisterBaseControl";
            this.Load += new System.EventHandler(this.RegisterBaseControl_Load);
            this.tlpLeft.ResumeLayout(false);
            this.tlpLeft.PerformLayout();
            this.pnlLeftTop.ResumeLayout(false);
            this.tlpBottom.ResumeLayout(false);
            this.tlpHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLeft;
        protected System.Windows.Forms.ListBox lsItems;
        private System.Windows.Forms.Panel pnlLeftTop;
        private MetroFramework.Controls.MetroTextBox txSearchString;
        private MetroFramework.Controls.MetroCheckBox ckHideNotActive;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private MetroFramework.Controls.MetroButton btCancel;
        private MetroFramework.Controls.MetroButton btSave;
        private MetroFramework.Controls.MetroButton btDelete;
        private MetroFramework.Controls.MetroButton btCopy;
        private MetroFramework.Controls.MetroButton btNew;
        private MetroFramework.Controls.MetroButton btEdit;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private MetroFramework.Controls.MetroLabel mlblItemHeading;
        private MetroFramework.Controls.MetroTile mtlReturn;
        private MetroFramework.Controls.MetroButton mbtnNextSearchResult;
    }
}
