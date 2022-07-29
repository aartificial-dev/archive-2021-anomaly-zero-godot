using Godot;
using System;

public class Player : KinematicBody { 

    private PlayerCamera _playerCamera;
    private RayCast _rayFloor;

    private float _jumpForce = 4f;
    private float _maxSpeed = 4f;
    private Vector3 _velocityMax = new Vector3(4f, 60f, 6f);
    private float _backVelMult = 0.6f;
    private float _gravity = 9.8f;
    private float _acceleration = 6f;
    private float _deceleration = 10f;
    private float _rotationY = 0f;

    private bool _isOnFloor = true;

    private Vector3 _velocity = Vector3.Zero;

    public override void _Ready() {
        _playerCamera = GetNode<PlayerCamera>("CameraHolder");
        _rayFloor = GetNode<RayCast>("RayFloor");
    }

	public override void _Process(float delta) {

	}

    public override void _PhysicsProcess(float delta) {
        _isOnFloor = _rayFloor.IsColliding() || IsOnFloor(); 
        ProcessMovement(delta);
    }

    private void ProcessMovement(float delta) {
        if (_isOnFloor) {
            _rotationY = _playerCamera.Rotation.y;
        }
        Vector2 moveDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        moveDir = moveDir.Normalized();

        Vector3 moveVector = Vector3.Zero;

        if (_isOnFloor) {
            moveVector.x = moveDir.x;
            moveVector.z = moveDir.y;

            _velocity.z = Mathf.Lerp(_velocity.z, _velocityMax.z * moveVector.z, Mathf.Abs(moveVector.z) * delta * _acceleration);
            _velocity.x = Mathf.Lerp(_velocity.x, _velocityMax.x * moveVector.x, Mathf.Abs(moveVector.x) * delta * _acceleration);
            _velocity.z = Mathf.Clamp(_velocity.z, -_velocityMax.z, _velocityMax.z * _backVelMult);
            _velocity.x = Mathf.Clamp(_velocity.x, -_velocityMax.x, _velocityMax.x);

            if (moveVector.z == 0f) {
                _velocity.z = Mathf.Lerp(_velocity.z, 0, _deceleration * delta);
            }
            if (moveVector.x == 0f) {
                _velocity.x = Mathf.Lerp(_velocity.x, 0, _deceleration * delta);
            }
            
            if (Input.IsActionJustPressed("jump")) {
                _velocity.y = _jumpForce;
            } else {
                _velocity.y = 0f;
            }
        } else {
            _velocity.y -= _gravity * delta;
        }
        _velocity.y = Mathf.Clamp(_velocity.y, -_velocityMax.y, _velocityMax.y);

        _velocity = _velocity.Rotated(Vector3.Up, _rotationY);
        _velocity = MoveAndSlide(linearVelocity: _velocity, upDirection: Vector3.Up, infiniteInertia: false);
        _velocity = _velocity.Rotated(Vector3.Up, -_rotationY);
        // _velocity.y = 0;
    }
}