using Godot;
using System;

class Weapon: Spatial {

    [Export]
    private AudioStream _asteamFire;
    [Export]
    private AudioStream _asteamFireCrit;
    [Export]
    private AudioStream _asteamFireAlt;
    [Export]
    private AudioStream _asteamReload;

    private AudioStreamPlayer _aplayerFire;
    private AudioStreamPlayer _aplayerFireCrit;
    private AudioStreamPlayer _aplayerFireAlt;
    private AudioStreamPlayer _aplayerReload;

    private Timer _fireTimer;

    private int _ammoCount = 99;
    private int _ammoLoaded = 12;
    private int _ammoMax = 12;

    public int AmmoCount {get => _ammoCount;}
    public int AmmoLoaded {get => _ammoLoaded;}
    public int AmmoMax {get => _ammoMax;}

    private RandomNumberGenerator rnd = new RandomNumberGenerator();

    private Control _ammoNode;
    private PlayerCamera _camera;

    public override void _Ready() {
        _aplayerFire = CreateStreamPlayer(_asteamFire);
        _aplayerFireCrit = CreateStreamPlayer(_asteamFireCrit);
        _aplayerReload = CreateStreamPlayer(_asteamReload);
        _aplayerFireAlt = CreateStreamPlayer(_asteamFireAlt);

        _fireTimer = new Timer(); AddChild(_fireTimer); _fireTimer.OneShot = true;

        rnd.Randomize();

        _ammoNode = (Control) GetTree().GetNodesInGroup("AmmoUI")[0];
        _ammoNode.GetNode<AmmoLabel>("AmmoInv").CurrentAmmo = _ammoCount;
        _ammoNode.GetNode<AmmoLabel>("AmmoActual").CurrentAmmo = _ammoLoaded;
        
        _camera = (PlayerCamera) GetTree().GetNodesInGroup("PlayerCamera")[0];
    }

    public override void _Process(float delta) {
        _ammoNode.GetNode<AmmoLabel>("AmmoInv").CurrentAmmo = _ammoCount;
        _ammoNode.GetNode<AmmoLabel>("AmmoActual").CurrentAmmo = _ammoLoaded;
        if (Input.IsActionJustPressed("fire") && _fireTimer.TimeLeft == 0 && _ammoLoaded > 0) {
            if (rnd.RandiRange(0, 8) == 8) {
                _aplayerFireCrit.Play();
            } else {
                _aplayerFire.Play();
            }
            _fireTimer.Start(0.3f);
            if (GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation != "idle")
                GetNode<AnimationPlayer>("AnimationPlayer").Stop();
            GetNode<AnimationPlayer>("AnimationPlayer").Play("fire");

            --_ammoLoaded;

            _ammoNode.GetNode<AmmoLabel>("AmmoActual").CurrentAmmo = _ammoLoaded;

            _camera.AddRecoil(0.1f);
            
        }
        if (Input.IsActionPressed("fire_alt") && _fireTimer.TimeLeft == 0 && _ammoLoaded > 0) {
            if (rnd.RandiRange(0, 16) == 16) {
                _aplayerFire.Play();
            } else {
                _aplayerFireAlt.Play();
            }
            _fireTimer.Start(0.15f);
            if (GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation != "idle")
                GetNode<AnimationPlayer>("AnimationPlayer").Stop();
            GetNode<AnimationPlayer>("AnimationPlayer").Play("fire_alt");

            --_ammoLoaded;

            _ammoNode.GetNode<AmmoLabel>("AmmoActual").CurrentAmmo = _ammoLoaded;

            _camera.AddRecoil(0.15f);
            
        }
        if (Input.IsActionJustPressed("reload")) {
            _aplayerReload.Play();
            GetNode<AnimationPlayer>("AnimationPlayer").Play("reload");

            int loada = Math.Min(_ammoMax, _ammoCount);
            _ammoCount -= loada;
            _ammoLoaded = loada;
            _ammoNode.GetNode<AmmoLabel>("AmmoInv").CurrentAmmo = _ammoCount;
            _ammoNode.GetNode<AmmoLabel>("AmmoActual").CurrentAmmo = _ammoLoaded;
        }
    }

    private AudioStreamPlayer CreateStreamPlayer(AudioStream stream) {
        AudioStreamPlayer player = new AudioStreamPlayer();
        player.Stream = stream;
        AddChild(player);
        return player;
    } 
}