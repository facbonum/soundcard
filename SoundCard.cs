using Godot;
using System;

public partial class SoundCard : Object  // Not a Node
{
	public float Frequency { get; set; } = 440f;   // Hz
	public float Volume { get; set; } = 0.3f;
	public float Crunchiness { get; set; } = 0.7f;

	public float GenerateSample(float phase)
	{
		float sample = (float)Math.Sin(phase * 2.0 * Math.PI) * Volume;
		sample = (float)Math.Round(sample * 15f) / 15f * Crunchiness;
		return sample;
	}
}
