namespace UiAvatingExample;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        await Task.Delay(1_000);
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
            textBox1.Text = t.Result;//Exception
        });
    }

    private void button3_Click(object sender, EventArgs e)
    {
        var getText = GetTextAsync();

        textBox1.BeginInvoke(new Action(() =>
        {
            textBox1.Text = getText.Result;
        }));
        //OR
        //textBox1.Invoke(new Action(() =>
        //{
        //    textBox1.Text = getText.Result;
        //}));
    }

    private void button4_Click(object sender, EventArgs e)
    {
        GetTextAsync().ContinueWith((t, o) =>
        {
            textBox1.Text = t.Result;

        }, TaskContinuationOptions.RunContinuationsAsynchronously, TaskScheduler.FromCurrentSynchronizationContext());//Sync with execution context
    }
}