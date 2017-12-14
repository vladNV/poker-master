namespace TexasHoldemClient
{
    partial class PokerWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.player1 = new System.Windows.Forms.Label();
            this.player2 = new System.Windows.Forms.Label();
            this.tableCard = new System.Windows.Forms.Label();
            this.player1cards = new System.Windows.Forms.Label();
            this.player2cards = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.winner = new System.Windows.Forms.Label();
            this.youLogin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(541, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 39);
            this.button1.TabIndex = 3;
            this.button1.Text = "Check";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(541, 206);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 41);
            this.button2.TabIndex = 4;
            this.button2.Text = "Call";
            this.button2.UseCompatibleTextRendering = true;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(541, 275);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 39);
            this.button3.TabIndex = 5;
            this.button3.Text = "Fold";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(541, 180);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(96, 20);
            this.textBox1.TabIndex = 6;
            // 
            // player1
            // 
            this.player1.AutoSize = true;
            this.player1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.player1.Location = new System.Drawing.Point(239, 381);
            this.player1.Name = "player1";
            this.player1.Size = new System.Drawing.Size(25, 25);
            this.player1.TabIndex = 7;
            this.player1.Text = "?";
            // 
            // player2
            // 
            this.player2.AutoSize = true;
            this.player2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.player2.Location = new System.Drawing.Point(251, 9);
            this.player2.Name = "player2";
            this.player2.Size = new System.Drawing.Size(25, 25);
            this.player2.TabIndex = 8;
            this.player2.Text = "?";
            // 
            // tableCard
            // 
            this.tableCard.AutoSize = true;
            this.tableCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tableCard.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableCard.Location = new System.Drawing.Point(29, 198);
            this.tableCard.Name = "tableCard";
            this.tableCard.Size = new System.Drawing.Size(80, 15);
            this.tableCard.TabIndex = 11;
            this.tableCard.Text = "Card on table";
            // 
            // player1cards
            // 
            this.player1cards.AutoSize = true;
            this.player1cards.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player1cards.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.player1cards.Location = new System.Drawing.Point(241, 336);
            this.player1cards.Name = "player1cards";
            this.player1cards.Size = new System.Drawing.Size(21, 15);
            this.player1cards.TabIndex = 12;
            this.player1cards.Text = "??";
            // 
            // player2cards
            // 
            this.player2cards.AutoSize = true;
            this.player2cards.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player2cards.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.player2cards.Location = new System.Drawing.Point(253, 50);
            this.player2cards.Name = "player2cards";
            this.player2cards.Size = new System.Drawing.Size(21, 15);
            this.player2cards.TabIndex = 13;
            this.player2cards.Text = "??";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.status.Location = new System.Drawing.Point(538, 39);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(43, 13);
            this.status.TabIndex = 14;
            this.status.Text = "Status: ";
            // 
            // winner
            // 
            this.winner.AutoSize = true;
            this.winner.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.winner.Location = new System.Drawing.Point(538, 335);
            this.winner.Name = "winner";
            this.winner.Size = new System.Drawing.Size(49, 13);
            this.winner.TabIndex = 15;
            this.winner.Text = "Winners ";
            // 
            // youLogin
            // 
            this.youLogin.AutoSize = true;
            this.youLogin.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.youLogin.Location = new System.Drawing.Point(538, 72);
            this.youLogin.Name = "youLogin";
            this.youLogin.Size = new System.Drawing.Size(57, 13);
            this.youLogin.TabIndex = 16;
            this.youLogin.Text = "Your login:";
            // 
            // PokerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(694, 415);
            this.Controls.Add(this.youLogin);
            this.Controls.Add(this.winner);
            this.Controls.Add(this.status);
            this.Controls.Add(this.player2cards);
            this.Controls.Add(this.player1cards);
            this.Controls.Add(this.tableCard);
            this.Controls.Add(this.player2);
            this.Controls.Add(this.player1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "PokerWindow";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.global_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label player1;
        private System.Windows.Forms.Label player2;
        private System.Windows.Forms.Label tableCard;
        private System.Windows.Forms.Label player1cards;
        private System.Windows.Forms.Label player2cards;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label winner;
        private System.Windows.Forms.Label youLogin;
    }
}

