using System;

public class StateVariable<T> {
    public T Value = default(T);
    public T PreviousValue = default(T);
    private string name;
    public bool HasChanged;

    public StateVariable(string name) {
        this.name = name;
    }

    public StateVariable(T value) {
        Value = value;
    }

    public StateVariable() {
    }

    public bool SetValue(T newValue, bool updateState = true) {
        Value = newValue;
        HasChanged = !Value.Equals(PreviousValue) && updateState;
        PreviousValue = newValue;
        return HasChanged;
    }

    public bool SetValue(T newValue, string name) {
        if (this.name != null && name != this.name)
            return false;
        Value = newValue;
        HasChanged = !Value.Equals(PreviousValue);
        PreviousValue = newValue;
        return HasChanged;
    }
}