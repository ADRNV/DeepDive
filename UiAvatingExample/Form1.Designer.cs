namespace UiAvatingExample;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
        button1 = new Button();
        button2 = new Button();
        textBox1 = new TextBox();
        button3 = new Button();
        button4 = new Button();
        button5 = new Button();
        button6 = new Button();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(12, 12);
        button1.Name = "button1";
        button1.Size = new Size(100, 23);
        button1.TabIndex = 0;
        button1.Text = "Async handling";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Location = new Point(118, 12);
        button2.Name = "button2";
        button2.Size = new Size(83, 25);
        button2.TabIndex = 1;
        button2.Text = "Task as sync ";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // textBox1
        // 
        textBox1.Location = new Point(12, 70);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(259, 23);
        textBox1.TabIndex = 2;
        textBox1.Text = "Result";
        // 
        // button3
        // 
        button3.Location = new Point(12, 41);
        button3.Name = "button3";
        button3.Size = new Size(137, 25);
        button3.TabIndex = 3;
        button3.Text = "Async legacy handling";
        button3.UseVisualStyleBackColor = true;
        button3.Click += button3_Click;
        // 
        // button4
        // 
        button4.Location = new Point(155, 43);
        button4.Name = "button4";
        button4.Size = new Size(100, 25);
        button4.TabIndex = 4;
        button4.Text = "SyncCtx";
        button4.UseVisualStyleBackColor = true;
        button4.Click += button4_Click;
        // 
        // button5
        // 
        button5.Location = new Point(261, 43);
        button5.Name = "button5";
        button5.Size = new Size(83, 25);
        button5.TabIndex = 5;
        button5.Text = "Custom Ctx";
        button5.UseVisualStyleBackColor = true;
        button5.Click += button5_Click;
        // 
        // button6
        // 
        button6.Location = new Point(207, 12);
        button6.Name = "button6";
        button6.Size = new Size(106, 25);
        button6.TabIndex = 6;
        button6.Text = "Task as awaitf";
        button6.UseVisualStyleBackColor = true;
        button6.Click += button6_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(440, 166);
        Controls.Add(button6);
        Controls.Add(button5);
        Controls.Add(button4);
        Controls.Add(button3);
        Controls.Add(textBox1);
        Controls.Add(button2);
        Controls.Add(button1);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;

    #endregion

    private TextBox textBox1;
    private Button button3;
    private Button button4;
    private Button button5;
    private Button button6;
}