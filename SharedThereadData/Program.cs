var _leftOperand = 40;
var _leftOperandLock = new Lock();

var _rightOperand = 2;
var _rightOperandLock = new Lock();

int _result = 0;
var resultLock = new Lock();

int Multiply()
{
    lock (_leftOperandLock)
    {
        lock (_rightOperandLock)
        {
            return _leftOperand * _rightOperand;
        }
    }
}
int Add()
{
    lock (_rightOperandLock)
    {
        lock (_leftOperandLock)
        {
            return _leftOperand * _rightOperand;
        }
    }
}

var multiplyThread = new Thread(() => {
    _result = Multiply();
});

var addThread = new Thread(() => {
    _result = Add();
});

var addThreadSecond = new Thread(() => {
    _result = Add();
});

multiplyThread.Start();
addThread.Start();
addThreadSecond.Start();

while (true)
{
    Console.WriteLine($"Result: {_result}");
    Console.Clear();
}

