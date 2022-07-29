using Godot;
using System;

public class CameraRecoil : Spatial { 
    
    [Export]
    private float maxRecoil = 1f;

    private float targetRecoil = 0f;

    public override void _Ready() {

    }

	public override void _Process(float delta) {
        targetRecoil -= delta;
        targetRecoil = Mathf.Clamp(targetRecoil, 0, maxRecoil);

        Vector3 rot = Rotation;
        rot.x = Mathf.LerpAngle(rot.x, targetRecoil, delta * 10f);
        Rotation = rot;
	}

    public void AddRecoil(float recoilStrength) {
        targetRecoil += recoilStrength;
    }
}