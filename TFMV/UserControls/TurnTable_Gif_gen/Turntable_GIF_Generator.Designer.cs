namespace TFMV.UserControls
{
    partial class Turntable_GIF_Generator
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
            this.panel61 = new System.Windows.Forms.Panel();
            this.label54 = new System.Windows.Forms.Label();
            this.lab_close = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel_close = new System.Windows.Forms.Panel();
            this.btn_start_turntable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtb_move_x_factor = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_invert_rotation = new System.Windows.Forms.CheckBox();
            this.panel61.SuspendLayout();
            this.panel_close.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel61
            // 
            this.panel61.BackColor = System.Drawing.Color.DimGray;
            this.panel61.Controls.Add(this.label54);
            this.panel61.Location = new System.Drawing.Point(1, 0);
            this.panel61.Name = "panel61";
            this.panel61.Size = new System.Drawing.Size(860, 25);
            this.panel61.TabIndex = 24;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label54.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label54.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label54.Location = new System.Drawing.Point(3, 3);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(243, 20);
            this.label54.TabIndex = 0;
            this.label54.Text = "Turntable GIF generator Tool";
            // 
            // lab_close
            // 
            this.lab_close.AutoSize = true;
            this.lab_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lab_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lab_close.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lab_close.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lab_close.Location = new System.Drawing.Point(7, 5);
            this.lab_close.Name = "lab_close";
            this.lab_close.Size = new System.Drawing.Size(16, 15);
            this.lab_close.TabIndex = 1;
            this.lab_close.Text = "X";
            this.lab_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // progressBar
            // 
            this.progressBar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.progressBar.Location = new System.Drawing.Point(1, 493);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(891, 10);
            this.progressBar.TabIndex = 28;
            // 
            // panel_close
            // 
            this.panel_close.BackColor = System.Drawing.Color.DimGray;
            this.panel_close.Controls.Add(this.lab_close);
            this.panel_close.Location = new System.Drawing.Point(862, 0);
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(29, 25);
            this.panel_close.TabIndex = 25;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // btn_start_turntable
            // 
            this.btn_start_turntable.BackColor = System.Drawing.Color.LightGray;
            this.btn_start_turntable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_start_turntable.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_start_turntable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btn_start_turntable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_start_turntable.Location = new System.Drawing.Point(266, 366);
            this.btn_start_turntable.Name = "btn_start_turntable";
            this.btn_start_turntable.Size = new System.Drawing.Size(297, 63);
            this.btn_start_turntable.TabIndex = 27;
            this.btn_start_turntable.Text = "Start";
            this.btn_start_turntable.UseVisualStyleBackColor = false;
            this.btn_start_turntable.Click += new System.EventHandler(this.btn_start_turntable_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(102, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(661, 16);
            this.label1.TabIndex = 29;
            this.label1.Text = "This tool captures a 360 degree horizontal turn around the object and generates a" +
    "n animated GIF.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(80, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(711, 16);
            this.label2.TabIndex = 29;
            this.label2.Text = "Please do not touch the mouse or keyboard while the tool is capturing the turntab" +
    "le screenshots.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(240, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(385, 16);
            this.label3.TabIndex = 29;
            this.label3.Text = "You will find the generated GIF in the screenshots folder.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(366, 16);
            this.label4.TabIndex = 29;
            this.label4.Text = "1-Place the object at the center of the HLMV screen.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(24, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(495, 16);
            this.label5.TabIndex = 29;
            this.label5.Text = "2-Place the mouse pointer in the center of the object and HLMV window.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(24, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(845, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "3-Click and hold the left mouse button and move the mouse to the right to test if" +
    " the object rotates while staying on screen.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(24, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 16);
            this.label7.TabIndex = 29;
            this.label7.Text = "Instructions:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(24, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(808, 16);
            this.label8.TabIndex = 29;
            this.label8.Text = "4-Move the camera/object until you can do a 360 rotation with the object staying " +
    "within the boundaries of the window.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(2, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(890, 65);
            this.panel1.TabIndex = 30;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(1, 273);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(891, 65);
            this.panel2.TabIndex = 30;
            // 
            // txtb_move_x_factor
            // 
            this.txtb_move_x_factor.BackColor = System.Drawing.Color.Gainsboro;
            this.txtb_move_x_factor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtb_move_x_factor.Location = new System.Drawing.Point(837, 444);
            this.txtb_move_x_factor.Name = "txtb_move_x_factor";
            this.txtb_move_x_factor.Size = new System.Drawing.Size(41, 20);
            this.txtb_move_x_factor.TabIndex = 31;
            this.txtb_move_x_factor.Text = "7";
            this.txtb_move_x_factor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtb_move_x_factor.TextChanged += new System.EventHandler(this.txtb_move_x_factor_TextChanged);
            this.txtb_move_x_factor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtb_move_x_factor_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(751, 448);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Animation speed";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(24, 224);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(854, 16);
            this.label9.TabIndex = 29;
            this.label9.Text = "Note: that some models/objects are not well centered, therefore it is hard or imp" +
    "ossible to do a center and symetric  360 turn.";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkGray;
            this.panel3.Location = new System.Drawing.Point(889, -3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 508);
            this.panel3.TabIndex = 31;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(24, 245);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(640, 16);
            this.label10.TabIndex = 29;
            this.label10.Text = "Tip: set a lower FOV value in HLMV (bottom left) to get a better perspective for " +
    "the turntable.";
            // 
            // cb_invert_rotation
            // 
            this.cb_invert_rotation.AutoSize = true;
            this.cb_invert_rotation.Location = new System.Drawing.Point(754, 470);
            this.cb_invert_rotation.Name = "cb_invert_rotation";
            this.cb_invert_rotation.Size = new System.Drawing.Size(134, 17);
            this.cb_invert_rotation.TabIndex = 33;
            this.cb_invert_rotation.Text = "Invert rotation direction";
            this.cb_invert_rotation.UseVisualStyleBackColor = true;
            this.cb_invert_rotation.CheckedChanged += new System.EventHandler(this.cb_invert_rotation_CheckedChanged);
            // 
            // Turntable_GIF_Generator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.cb_invert_rotation);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtb_move_x_factor);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel61);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.panel_close);
            this.Controls.Add(this.btn_start_turntable);
            this.MaximumSize = new System.Drawing.Size(892, 503);
            this.MinimumSize = new System.Drawing.Size(892, 503);
            this.Name = "Turntable_GIF_Generator";
            this.Size = new System.Drawing.Size(892, 503);
            this.Load += new System.EventHandler(this.Turntable_GIF_Generator_Load);
            this.panel61.ResumeLayout(false);
            this.panel61.PerformLayout();
            this.panel_close.ResumeLayout(false);
            this.panel_close.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel61;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label lab_close;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel_close;
        private System.Windows.Forms.Button btn_start_turntable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtb_move_x_factor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cb_invert_rotation;
    }
}
