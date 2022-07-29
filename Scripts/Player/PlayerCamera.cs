using Godot;
using System;

public class PlayerCamera : Spatial { 

    private float mouseSens = 1;
    private float mouseSensHorizontal = 1;
    private float mouseSensVertical = 1;
    private float mouseSensNormal = 0.001f;

    private bool mouseXInversed = false;
    private bool mouseYInversed = false;

    private Camera _camera;
    public Camera Camera {get => _camera;}

    private Player _player;

    private CameraRecoil _recoil;

    public override void _Ready() {
        _camera = GetNode<Camera>("Recoil/Camera");
        _recoil = GetNode<CameraRecoil>("Recoil");
        _player = GetParent<Player>();

        Input.SetMouseMode(Input.MouseMode.Captured);

        foreach (Spatial mirror in GetTree().GetNodesInGroup("Mirror")) {
            mirror.Set("MainCam", _camera);
        }
    }

	public override void _Process(float delta) {
        RayCast gunRay = GetNode<RayCast>("Recoil/GunRayCast");
        Weapon gun = GetNode<Weapon>("Recoil/Spatial");
        // TODO add weapon pivot and transition with lookat and sway
        if (gunRay.IsColliding()) {
            gun.LookAt(gunRay.GetCollisionPoint(), Vector3.Up);
        } else {
            gun.LookAt(_camera.GlobalTransform.origin - GlobalTransform.basis.z * 20, Vector3.Up);
        }
	}

    public override void _Input(InputEvent e) {
        // GD.Print(e);
        if (e is InputEventMouseMotion motion && Input.GetMouseMode() == Input.MouseMode.Captured) {
            Vector2 rel = motion.Relative;

            Vector3 rot = this.Rotation;

            rot.x -= rel.y * mouseSens * mouseSensVertical * mouseSensNormal * (mouseYInversed ? -1 : 1);
            rot.y -= rel.x * mouseSens * mouseSensHorizontal * mouseSensNormal * (mouseXInversed ? -1 : 1);

            rot.x = Mathf.Clamp(rot.x, -Mathf.Pi / 2f, Mathf.Pi / 2f);
            rot.y = Mathf.Wrap(rot.y, 0, Mathf.Tau);

            this.Rotation = rot;

            // TODO compass
            // ui.SetCompass(-rot.y);
        }
    }

    public void AddRecoil(float recoilStrength) {
        _recoil.AddRecoil(recoilStrength);
    }
    
}