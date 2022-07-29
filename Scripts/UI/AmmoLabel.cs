using Godot;
using System;

[Tool]
public class AmmoLabel : Control { 
    
    [Export]
    public int MaxAmmo {
        set { maxAmmo = Math.Max(value, 0); OnAmmoChange(); } 
        get { return maxAmmo; }
    }
    [Export]
    public int CurrentAmmo {
        set { currentAmmo = value; OnAmmoChange(); } 
        get { return currentAmmo; }
    }

    private int currentAmmo = 0;
    private int maxAmmo = 99;

    private Label label;

    public override void _Ready() {
        label = GetNode<Label>("Value");
    }

	public override void _Process(float delta) {

	}

    private void OnAmmoChange() {
        if (label is null) return;
        currentAmmo = Math.Min(Math.Max(currentAmmo, 0), maxAmmo);

        label.Text = currentAmmo.ToString();
    }
}