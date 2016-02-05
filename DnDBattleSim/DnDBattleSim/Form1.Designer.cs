namespace DnDBattleSim
{
    partial class Form1
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
            this.StepList = new System.Windows.Forms.ListBox();
            this.start = new System.Windows.Forms.TextBox();
            this.goal = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // StepList
            // 
            this.StepList.FormattingEnabled = true;
            this.StepList.Location = new System.Drawing.Point(12, 50);
            this.StepList.Name = "StepList";
            this.StepList.Size = new System.Drawing.Size(469, 186);
            this.StepList.TabIndex = 0;
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(12, 12);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(100, 20);
            this.start.TabIndex = 1;
            // 
            // goal
            // 
            this.goal.Location = new System.Drawing.Point(150, 12);
            this.goal.Name = "goal";
            this.goal.Size = new System.Drawing.Size(100, 20);
            this.goal.TabIndex = 2;
            this.goal.TextChanged += new System.EventHandler(this.goal_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 261);
            this.Controls.Add(this.goal);
            this.Controls.Add(this.start);
            this.Controls.Add(this.StepList);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox StepList;
        private System.Windows.Forms.TextBox start;
        private System.Windows.Forms.TextBox goal;
    }
}

