namespace UiAvatingExample;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        textBox1.Text = await GetTextAsync();
    }

    private async Task<string> GetTextAsync()
    {
         return await Task.FromResult(Guid.NewGuid().ToString());
    }

    private void button2_Click(object sender, EventArgs e)
    {
        GetTextAsync().ContinueWith((t) =>
        {
            textBox1.Text = t.Result;
        });
    }

}