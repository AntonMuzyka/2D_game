using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement_Controller))]
public class PC_input_controler : MonoBehaviour
{
    Movement_Controller _playerMovement;

    bool _rolling;
    float _move;
    bool _jump;
    bool _sit;

    private void Start()
    {
        _playerMovement = GetComponent<Movement_Controller>();
    }
    void Update()
    {
        _move = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }

        if (_rolling = Input.GetKey(KeyCode.LeftAlt))
            _playerMovement.StartRoll();

        _sit = Input.GetKey(KeyCode.C); //для присідання в майбутньому

        if (Input.GetKey(KeyCode.K))
            _playerMovement.StartCast();

        if (Input.GetKey(KeyCode.J))
        {
            _playerMovement.StartStrike();
            _playerMovement.StartAirAtk();
        }
    }
    private void FixedUpdate()
    {
        _playerMovement.Move(_move, _jump, _rolling, _sit);
        _jump = false;
    }
}
