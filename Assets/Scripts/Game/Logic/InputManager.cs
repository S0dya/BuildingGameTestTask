using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Zenject;
using System;

public class InputManager : SubjectMonoBehaviour
{
    private Inputs _input;

    private InputActionMap _inGameInput;

    private List<InputActionMap> _actionMapsList = new();

    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
        });
    }

    private void Start()
    {
        _inGameInput = _input.InGameInput;

        _actionMapsList.Add(_inGameInput);

        ToGameInput();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _input = new Inputs();

        //in game
        _input.InGameInput.Move.performed += ctx => _player.InputMove(ctx.ReadValue<Vector2>());
        _input.InGameInput.Move.canceled += ctx => _player.InputMoveStopped();

        _input.InGameInput.Look.performed += ctx => _player.InputLook(ctx.ReadValue<Vector2>());

        _input.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //in game
        _input.InGameInput.Move.performed -= ctx => _player.InputMove(ctx.ReadValue<Vector2>());
        _input.InGameInput.Move.canceled -= ctx => _player.InputMoveStopped();

        _input.InGameInput.Look.performed -= ctx => _player.InputLook(ctx.ReadValue<Vector2>());

        _input.Disable();
    }

    private void ToGameInput() => EnableActionMap(_inGameInput);

    private void EnableActionMap(InputActionMap mapToEnable)
    {
        DisableMaps();

        mapToEnable.Enable();
    }

    private void DisableMaps()
    {
        foreach (InputActionMap map in _actionMapsList) map.Disable();
    }
}