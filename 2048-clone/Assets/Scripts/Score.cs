using System;

public class Score
{
    private int _value;
    public int value
    {
        get => _value;
        set
        {
            _value = value;
            changed?.Invoke(_value);
        }
    }

    public Action<int> changed;
}