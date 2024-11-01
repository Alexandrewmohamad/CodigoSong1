namespace PnT.SongClient.UI.Controls
{
    partial class StatisticsClassMonthControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsClassMonthControl));
            this.tlpClassMontData = new System.Windows.Forms.TableLayoutPanel();
            this.mlblNumEvasion = new MetroFramework.Controls.MetroLabel();
            this.mlblNumEvasionValue = new MetroFramework.Controls.MetroLabel();
            this.mlblMonth = new MetroFramework.Controls.MetroLabel();
            this.mlblPercentEvasion = new MetroFramework.Controls.MetroLabel();
            this.mlblPercentEvasionValue = new MetroFramework.Controls.MetroLabel();
            this.tlpClassMontData.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpClassMontData
            // 
            resources.ApplyResources(this.tlpClassMontData, "tlpClassMontData");
            this.tlpClassMontData.Controls.Add(this.mlblNumEvasion, 0, 2);
            this.tlpClassMontData.Controls.Add(this.mlblNumEvasionValue, 1, 2);
            this.tlpClassMontData.Controls.Add(this.mlblMonth, 0, 1);
            this.tlpClassMontData.Controls.Add(this.mlblPercentEvasion, 0, 3);
            this.tlpClassMontData.Controls.Add(this.mlblPercentEvasionValue, 1, 3);
            this.tlpClassMontData.Name = "tlpClassMontData";
            // 
            // mlblNumEvasion
            // 
            resources.ApplyResources(this.mlblNumEvasion, "mlblNumEvasion");
            this.mlblNumEvasion.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblNumEvasion.Name = "mlblNumEvasion";
            this.mlblNumEvasion.UseCustomBackColor = true;
            // 
            // mlblNumEvasionValue
            // 
            resources.ApplyResources(this.mlblNumEvasionValue, "mlblNumEvasionValue");
            this.mlblNumEvasionValue.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblNumEvasionValue.Name = "mlblNumEvasionValue";
            this.mlblNumEvasionValue.UseCustomBackColor = true;
            // 
            // mlblMonth
            // 
            resources.ApplyResources(this.mlblMonth, "mlblMonth");
            this.mlblMonth.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblMonth.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.mlblMonth.Name = "mlblMonth";
            this.mlblMonth.UseCustomBackColor = true;
            // 
            // mlblPercentEvasion
            // 
            resources.ApplyResources(this.mlblPercentEvasion, "mlblPercentEvasion");
            this.mlblPercentEvasion.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPercentEvasion.Name = "mlblPercentEvasion";
            this.mlblPercentEvasion.UseCustomBackColor = true;
            // 
            // mlblPercentEvasionValue
            // 
            resources.ApplyResources(this.mlblPercentEvasionValue, "mlblPercentEvasionValue");
            this.mlblPercentEvasionValue.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPercentEvasionValue.Name = "mlblPercentEvasionValue";
            this.mlblPercentEvasionValue.UseCustomBackColor = true;
            // 
            // StatisticsClassMonthControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpClassMontData);
            this.Name = "StatisticsClassMonthControl";
            this.tlpClassMontData.ResumeLayout(false);
            this.tlpClassMontData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpClassMontData;
        private MetroFramework.Controls.MetroLabel mlblNumEvasion;
        private MetroFramework.Controls.MetroLabel mlblNumEvasionValue;
        private MetroFramework.Controls.MetroLabel mlblMonth;
        private MetroFramework.Controls.MetroLabel mlblPercentEvasion;
        private MetroFramework.Controls.MetroLabel mlblPercentEvasionValue;
    }
}
