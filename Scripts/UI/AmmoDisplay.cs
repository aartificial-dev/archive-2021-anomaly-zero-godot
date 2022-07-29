using Godot;
using System;

public class AmmoDisplay : Control { 
    private Label ammoActial;
    private Label ammoInventory;
    private Label ammoType;
    private Label fireMode;

    public override void _Ready() {
        ammoActial = GetNode<Label>("AmmoActual");
        ammoInventory = GetNode<Label>("AmmoMax");
        ammoType = GetNode<Label>("AmmoType");
        fireMode = GetNode<Label>("FireMode");
    }

    public void SetAmmoType(String type) {
        ammoType.Text = type;
    }

    public void SetAmmo(int ammo) {
        ammoActial.Text = ammo.ToString();
    }

    public void SetAmmoAvailable(int ammo) {
        ammoInventory.Text = ammo.ToString();
    }

    public void SetFireMode(uint flag) {
        if ((flag & 0b001) != 0b0) {
            fireMode.Text = "semi";
        } else 
        if ((flag & 0b010) != 0b0) {
            fireMode.Text = "burst";
        } else 
        if ((flag & 0b100) != 0b0) {
            fireMode.Text = "auto";
        } else {
            fireMode.Text = "one";
        }
    }
}