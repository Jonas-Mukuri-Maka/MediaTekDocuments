namespace MediaTekDocuments.view
{
    partial class frmAlerteAbonnement
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
            this.lvAbonnementsEcheance = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAbonneOk = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvAbonnementsEcheance
            // 
            this.lvAbonnementsEcheance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lvAbonnementsEcheance.GridLines = true;
            this.lvAbonnementsEcheance.HideSelection = false;
            this.lvAbonnementsEcheance.Location = new System.Drawing.Point(12, 109);
            this.lvAbonnementsEcheance.Name = "lvAbonnementsEcheance";
            this.lvAbonnementsEcheance.Size = new System.Drawing.Size(553, 114);
            this.lvAbonnementsEcheance.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.lvAbonnementsEcheance.TabIndex = 0;
            this.lvAbonnementsEcheance.UseCompatibleStateImageBehavior = false;
            this.lvAbonnementsEcheance.View = System.Windows.Forms.View.List;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(47, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(466, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Les abonnement concernés ce termine en moins de 30 jours:";
            // 
            // btnAbonneOk
            // 
            this.btnAbonneOk.Location = new System.Drawing.Point(210, 229);
            this.btnAbonneOk.Name = "btnAbonneOk";
            this.btnAbonneOk.Size = new System.Drawing.Size(146, 65);
            this.btnAbonneOk.TabIndex = 2;
            this.btnAbonneOk.Text = "OK";
            this.btnAbonneOk.UseVisualStyleBackColor = true;
            this.btnAbonneOk.Click += new System.EventHandler(this.btnAbonneOk_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(235, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 39);
            this.label2.TabIndex = 3;
            this.label2.Text = "Alerte";
            // 
            // frmAlerteAbonnement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 325);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAbonneOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvAbonnementsEcheance);
            this.Name = "frmAlerteAbonnement";
            this.Text = "frmAlerteAbonnement";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvAbonnementsEcheance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAbonneOk;
        private System.Windows.Forms.Label label2;
    }
}