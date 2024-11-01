namespace PnT.SongClient.UI.Controls
{
    partial class StatisticsAttendanceMonthControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsAttendanceMonthControl));
            this.tlpClassMontData = new System.Windows.Forms.TableLayoutPanel();
            this.mlblPercentPresence = new MetroFramework.Controls.MetroLabel();
            this.mlblPercentPresenceValue = new MetroFramework.Controls.MetroLabel();
            this.mlblMonth = new MetroFramework.Controls.MetroLabel();
            this.mlblNumPresences = new MetroFramework.Controls.MetroLabel();
            this.mlblNumPresencesValue = new MetroFramework.Controls.MetroLabel();
            this.mlblNumAbsences = new MetroFramework.Controls.MetroLabel();
            this.mlblNumAbsencesValue = new MetroFramework.Controls.MetroLabel();
            this.tlpClassMontData.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpClassMontData
            // 
            resources.ApplyResources(this.tlpClassMontData, "tlpClassMontData");
            this.tlpClassMontData.Controls.Add(this.mlblPercentPresence, 0, 2);
            this.tlpClassMontData.Controls.Add(this.mlblPercentPresenceValue, 1, 2);
            this.tlpClassMontData.Controls.Add(this.mlblMonth, 0, 1);
            this.tlpClassMontData.Controls.Add(this.mlblNumPresences, 0, 3);
            this.tlpClassMontData.Controls.Add(this.mlblNumPresencesValue, 1, 3);
            this.tlpClassMontData.Controls.Add(this.mlblNumAbsences, 0, 4);
            this.tlpClassMontData.Controls.Add(this.mlblNumAbsencesValue, 1, 4);
            this.tlpClassMontData.Name = "tlpClassMontData";
            // 
            // mlblPercentPresence
            // 
            resources.ApplyResources(this.mlblPercentPresence, "mlblPercentPresence");
            this.mlblPercentPresence.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPercentPresence.Name = "mlblPercentPresence";
            this.mlblPercentPresence.UseCustomBackColor = true;
            // 
            // mlblPercentPresenceValue
            // 
            resources.ApplyResources(this.mlblPercentPresenceValue, "mlblPercentPresenceValue");
            this.mlblPercentPresenceValue.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblPercentPresenceValue.Name = "mlblPercentPresenceValue";
            this.mlblPercentPresenceValue.UseCustomBackColor = true;
            // 
            // mlblMonth
            // 
            resources.ApplyResources(this.mlblMonth, "mlblMonth");
            this.mlblMonth.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblMonth.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.mlblMonth.Name = "mlblMonth";
            this.mlblMonth.UseCustomBackColor = true;
            // 
            // mlblNumPresences
            // 
            resources.ApplyResources(this.mlblNumPresences, "mlblNumPresences");
            this.mlblNumPresences.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblNumPresences.Name = "mlblNumPresences";
            this.mlblNumPresences.UseCustomBackColor = true;
            // 
            // mlblNumPresencesValue
            // 
            resources.ApplyResources(this.mlblNumPresencesValue, "mlblNumPresencesValue");
            this.mlblNumPresencesValue.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblNumPresencesValue.Name = "mlblNumPresencesValue";
            this.mlblNumPresencesValue.UseCustomBackColor = true;
            // 
            // mlblNumAbsences
            // 
            resources.ApplyResources(this.mlblNumAbsences, "mlblNumAbsences");
            this.mlblNumAbsences.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblNumAbsences.Name = "mlblNumAbsences";
            this.mlblNumAbsences.UseCustomBackColor = true;
            // 
            // mlblNumAbsencesValue
            // 
            resources.ApplyResources(this.mlblNumAbsencesValue, "mlblNumAbsencesValue");
            this.mlblNumAbsencesValue.FontSize = MetroFramework.MetroLabelSize.Small;
            this.mlblNumAbsencesValue.Name = "mlblNumAbsencesValue";
            this.mlblNumAbsencesValue.UseCustomBackColor = true;
            // 
            // StatisticsAttendanceMonthControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpClassMontData);
            this.Name = "StatisticsAttendanceMonthControl";
            this.tlpClassMontData.ResumeLayout(false);
            this.tlpClassMontData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpClassMontData;
        private MetroFramework.Controls.MetroLabel mlblPercentPresence;
        private MetroFramework.Controls.MetroLabel mlblPercentPresenceValue;
        private MetroFramework.Controls.MetroLabel mlblMonth;
        private MetroFramework.Controls.MetroLabel mlblNumPresences;
        private MetroFramework.Controls.MetroLabel mlblNumPresencesValue;
        private MetroFramework.Controls.MetroLabel mlblNumAbsences;
        private MetroFramework.Controls.MetroLabel mlblNumAbsencesValue;
    }
}
