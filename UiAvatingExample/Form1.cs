using System.Diagnostics;
using System.Threading.Tasks;

namespace UiAvatingExample;

public partial class Form1 : Form
{

    private SingleThreadSynchronizationContext _singleThreadContext;


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
        await Task.Delay(1_000);
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

    private void button5_Click(object sender, EventArgs e)
    {
        _singleThreadContext = new SingleThreadSynchronizationContext("MySingleThread");

        // Запускаем таски на одном потоке
        for (int i = 0; i < 5; i++)
        {
            var taskId = i;

            var call = GetTextAsync().ContinueWith((t, s) =>
            {
                textBox1.Text += $"Task {taskId} on thread {Thread.CurrentThread.ManagedThreadId}\r\n";

            }, _singleThreadContext);

            // Эта работа будет на одном потоке!
            //_singleThreadContext.Post(_ =>
            //{
            //    Debug.WriteLine($"Task {taskId} on thread {Thread.CurrentThread.ManagedThreadId}\r\n");
            //}, null);
        }
    }

    private async void button6_Click(object sender, EventArgs e)
    {
        var result = await GetTextAsync().ConfigureAwait(false);//Ignores SCTX and run on ThreadPoll
        textBox1.Text = result;
    }
}