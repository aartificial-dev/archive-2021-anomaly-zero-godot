using Godot;
using System;

[Tool]
public class DayTimeSystem : Spatial { 
    [Export]
    public GradientTexture sunGradient;
    [Export]
    public GradientTexture skyTopGradient;
    [Export]
    public GradientTexture skyHorizonGradient;
    [Export]
    public GradientTexture skyLightGradient;
    [Export]
    public GradientTexture skyShadowGradient;
    [Export(PropertyHint.Range, "0, 2400")]
    public float timeOfDay = 1200f;
    [Export]
    public float timeMultiplier = 0.1f;
    [Export]
    public bool isRunning = false;
    [Export]
    public bool updateSky = false;
    [Export]
    public bool autoStart = false;

    private float sunAngle = 0f; // 0 - at west, 180/-180 at east, -x night, +x day // sun moving from 180 to -180

    private WorldEnvironment worldEnvironment;
    private DirectionalLight light;
    private Godot.Environment environment;
    private ProceduralSky proceduralSky;

    public override void _Ready() {
        if (!Engine.EditorHint && autoStart) {
            isRunning = true;
        }

        worldEnvironment = GetNode<WorldEnvironment>("WorldEnvironment");
        environment = worldEnvironment.Environment;
        proceduralSky = (ProceduralSky) environment.BackgroundSky;

        light = GetNode<DirectionalLight>("DirectionalLight");
    }

	public override void _Process(float delta) {
        if (isRunning) {
            timeOfDay += delta * timeMultiplier;
            if (timeOfDay >= 2400f) {
                timeOfDay -= 2400f;
            }
            UpdateSky();
        }
        if (updateSky) {
            UpdateSky();
            updateSky = false;
        }
	}

    private void UpdateSky() {
        float x = timeOfDay / 2400f;
        proceduralSky.SunColor = sunGradient.Gradient.Interpolate(x);
        proceduralSky.SkyTopColor = skyTopGradient.Gradient.Interpolate(x);
        proceduralSky.SkyHorizonColor = skyHorizonGradient.Gradient.Interpolate(x);
        proceduralSky.SunLatitude = Mathf.Wrap(-90f - (x * 360f), -180, 180);

        proceduralSky.GroundHorizonColor = proceduralSky.SkyHorizonColor;
        // proceduralSky.GroundBottomColor = proceduralSky.SkyHorizonColor;

        environment.FogColor = proceduralSky.SkyHorizonColor;
        environment.FogSunColor = proceduralSky.SunColor;

        float latitude = 90f + (x * 360f);
        light.RotationDegrees = new Vector3(latitude, -120f, 0f);
        light.LightColor = skyLightGradient.Gradient.Interpolate(x);
        light.ShadowColor = skyShadowGradient.Gradient.Interpolate(x);
    }

    public String GetTime() {
        int hours = Mathf.FloorToInt(timeOfDay / 100);
        float minuteCount = timeOfDay - (hours * 100);
        int minutes = Mathf.FloorToInt( (minuteCount / 100) * 60 );

        String hour = hours.ToString();
        String minute = minutes.ToString();
        if (hour.Length == 1) hour = GD.Str(0, hour);
        if (minute.Length == 1) minute = GD.Str(0, minute);
        return GD.Str(hour, ":", minute);
    }
}