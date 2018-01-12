using UnityEngine;

public static class WinMRInput
{
    public enum Inputs
    {
        None,
        Trigger,
        Grip,
        Menu,
        TouchPad,
        Thumbstick,
        TouchPadUp,
        TouchPadDown,
        TouchPadLeft,
        TouchPadRight
    }

    static UnityEngine.XR.WSA.Input.InteractionSourceState[] _oldInputState = new UnityEngine.XR.WSA.Input.InteractionSourceState[0];
    static UnityEngine.XR.WSA.Input.InteractionSourceState[] _inputState = new UnityEngine.XR.WSA.Input.InteractionSourceState[0];

    static UnityEngine.XR.WSA.Input.InteractionSourceState GetInputState(UnityEngine.XR.WSA.Input.InteractionSourceState[] TabToSearch, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        foreach (UnityEngine.XR.WSA.Input.InteractionSourceState state in TabToSearch)
        {
            if (state.source.handedness == hand) return state;
        }
        return new UnityEngine.XR.WSA.Input.InteractionSourceState();
    }

    public static void Update()
    {
        _oldInputState = _inputState;
        _inputState = UnityEngine.XR.WSA.Input.InteractionManager.GetCurrentReading();
    }

    public static bool GetTracked(UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        foreach (UnityEngine.XR.WSA.Input.InteractionSourceState state in _inputState)
        {
            if (state.source.handedness == hand) return true;
        }
        return false;
    }

    public static bool GetPressed(Inputs input, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        UnityEngine.XR.WSA.Input.InteractionSourceState state = GetInputState(_inputState, hand);

        switch (input)
        {
            case Inputs.Menu:
                return state.menuPressed;

            case Inputs.Thumbstick:
                return state.thumbstickPressed;

            case Inputs.TouchPad:
                return state.touchpadPressed;

            case Inputs.Grip:
                return state.grasped;

            case Inputs.Trigger:
                return state.selectPressed;

            case Inputs.TouchPadUp:
                return state.touchpadPosition.y < -0.5f && state.touchpadPressed;

            case Inputs.TouchPadDown:
                return state.touchpadPosition.y > 0.5f && state.touchpadPressed;

            case Inputs.TouchPadLeft:
                return state.touchpadPosition.x < -0.5f && state.touchpadPressed;

            case Inputs.TouchPadRight:
                return state.touchpadPosition.x > 0.5f && state.touchpadPressed;

            default:
                return false;
        }
    }

    public static bool GetDown(Inputs input, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        UnityEngine.XR.WSA.Input.InteractionSourceState state = GetInputState(_inputState, hand);
        UnityEngine.XR.WSA.Input.InteractionSourceState oldState = GetInputState(_oldInputState, hand);

        switch (input)
        {
            case Inputs.Menu:
                return !oldState.menuPressed && state.menuPressed;

            case Inputs.Thumbstick:
                return !oldState.thumbstickPressed && state.thumbstickPressed;

            case Inputs.TouchPad:
                return !oldState.touchpadPressed && state.touchpadPressed;

            case Inputs.Grip:
                return !oldState.grasped && state.grasped;

            case Inputs.Trigger:
                return !oldState.selectPressed && state.selectPressed;

            case Inputs.TouchPadUp:
                return !oldState.touchpadPressed && state.touchpadPosition.y < -0.5f && state.touchpadPressed;

            case Inputs.TouchPadDown:
                return !oldState.touchpadPressed &&  state.touchpadPosition.y > 0.5f && state.touchpadPressed;

            case Inputs.TouchPadLeft:
                return !oldState.touchpadPressed && state.touchpadPosition.x < -0.5f && state.touchpadPressed;

            case Inputs.TouchPadRight:
                return !oldState.touchpadPressed && state.touchpadPosition.x > 0.5f && state.touchpadPressed;

            default:
                return false;
        }
    }

    public static bool GetUp(Inputs input, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        UnityEngine.XR.WSA.Input.InteractionSourceState state = GetInputState(_inputState, hand);
        UnityEngine.XR.WSA.Input.InteractionSourceState oldState = GetInputState(_oldInputState, hand);

        switch (input)
        {
            case Inputs.Menu:
                return oldState.menuPressed && !state.menuPressed;

            case Inputs.Thumbstick:
                return oldState.thumbstickPressed && !state.thumbstickPressed;

            case Inputs.TouchPad:
                return oldState.touchpadPressed && !state.touchpadPressed;

            case Inputs.Grip:
                return oldState.grasped && !state.grasped;

            case Inputs.Trigger:
                return oldState.selectPressed && !state.selectPressed;

            case Inputs.TouchPadUp:
                return oldState.touchpadPosition.y < -0.5f && oldState.touchpadPressed && !state.touchpadPressed;

            case Inputs.TouchPadDown:
                return oldState.touchpadPosition.y > 0.5f && oldState.touchpadPressed && !state.touchpadPressed;

            case Inputs.TouchPadLeft:
                return oldState.touchpadPosition.x < -0.5f && oldState.touchpadPressed && !state.touchpadPressed;

            case Inputs.TouchPadRight:
                return oldState.touchpadPosition.x > 0.5f && oldState.touchpadPressed && !state.touchpadPressed;

            default:
                return false;
        }
    }

    public static float GetAxis1D(Inputs input, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        UnityEngine.XR.WSA.Input.InteractionSourceState state = GetInputState(_inputState, hand);

        switch (input)
        {
            case Inputs.Menu:
                return state.menuPressed ? 1.0f : 0.0f;

            case Inputs.Thumbstick:
                return state.thumbstickPosition.y;

            case Inputs.TouchPad:
                return state.touchpadPosition.y;

            case Inputs.Grip:
                return state.grasped ? 1.0f : 0.0f;

            case Inputs.Trigger:
                return state.selectPressedAmount;

            default:
                return 0.0f;
        }
    }

    public static Vector2 GetAxis2D(Inputs input, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        UnityEngine.XR.WSA.Input.InteractionSourceState state = GetInputState(_inputState, hand);

        switch (input)
        {
            case Inputs.Menu:
                return state.menuPressed ? Vector2.one : Vector2.zero;

            case Inputs.Thumbstick:
                return state.thumbstickPosition;

            case Inputs.TouchPad:
                return state.touchpadPosition;

            case Inputs.Grip:
                return state.grasped ? Vector2.one : Vector2.zero;

            case Inputs.Trigger:
                return new Vector2(state.selectPressedAmount, 0.0f);

            default:
                return Vector2.zero;
        }
    }

    public static bool GetTouch(Inputs input, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        UnityEngine.XR.WSA.Input.InteractionSourceState state = GetInputState(_inputState, hand);

        switch (input)
        {
            case Inputs.TouchPad:
                return state.touchpadTouched;

            default:
                return false;
        }
    }

    public static bool GetTouchDown(Inputs input, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        UnityEngine.XR.WSA.Input.InteractionSourceState state = GetInputState(_inputState, hand);
        UnityEngine.XR.WSA.Input.InteractionSourceState oldState = GetInputState(_oldInputState, hand);

        switch (input)
        {
            case Inputs.TouchPad:
                return !oldState.touchpadTouched && state.touchpadTouched;

            default:
                return false;
        }
    }

    public static bool GetTouchUp(Inputs input, UnityEngine.XR.WSA.Input.InteractionSourceHandedness hand)
    {
        UnityEngine.XR.WSA.Input.InteractionSourceState state = GetInputState(_inputState, hand);
        UnityEngine.XR.WSA.Input.InteractionSourceState oldState = GetInputState(_oldInputState, hand);

        switch (input)
        {
            case Inputs.TouchPad:
                return oldState.touchpadTouched && !state.touchpadTouched;

            default:
                return false;
        }
    }
}
