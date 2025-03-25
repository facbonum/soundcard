using Godot;
using System;

public partial class AudioPlayer : Node3D
{
	private AudioStreamPlayer streamPlayer;
	private AudioStreamGenerator generator;
	private AudioStreamGeneratorPlayback playback;

	private float phase = 0f;
	private float sampleRate = 44100f;
	private float frequency = 440f;    // A4 frequency
	private float volume = 0.3f;
	private float crunchiness = 0.7f;  // Simulate low-quality DAC

	private Timer audioTimer;

	public override void _Ready()
	{
		GD.Print("AudioPlayer Ready");

		// Initialize the AudioStreamPlayer
		streamPlayer = new AudioStreamPlayer();
		AddChild(streamPlayer);

		// Create and configure the AudioStreamGenerator
		generator = new AudioStreamGenerator();
		generator.MixRate = sampleRate;
		generator.BufferLength = 0.1f;  // Low latency buffer
		streamPlayer.Stream = generator;

		// Start the audio stream
		streamPlayer.Play(0);  // Start at the beginning (0 seconds)
		playback = (AudioStreamGeneratorPlayback)streamPlayer.GetStreamPlayback();

		// Set up Timer for audio updates
		audioTimer = new Timer();
		AddChild(audioTimer);
		audioTimer.WaitTime = 0.1f;  // Update every 100ms (change as needed)
		audioTimer.OneShot = false;
		audioTimer.Connect("timeout", new Callable(this, nameof(OnAudioTimerTimeout)));  // Fixed this line
		audioTimer.Start();
	}

	private void OnAudioTimerTimeout()
	{
		if (playback == null)
			return;

		int bufferSize = playback.GetFramesAvailable();
		if (bufferSize == 0)
			return;

		var buffer = new Vector2[bufferSize];  // Stereo output (Vector2 for Left & Right)

		for (int i = 0; i < bufferSize; i++)
		{
			float sample = (float)Math.Sin(phase * 2.0 * Math.PI) * volume;

			// Simulate analog crunchiness by bit-crushing
			sample = (float)Math.Round(sample * 15f) / 15f * crunchiness;

			buffer[i] = new Vector2(sample, sample);  // Left & Right channels

			phase += frequency / sampleRate;
			if (phase >= 1.0f)
				phase -= 1.0f;
		}

		playback.PushBuffer(buffer);  // Fixed this line
	}
}
