using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatGenerator : MonoBehaviour {
	public Player player;

	public GameObject score;
	private float scoreBaseY;

	public GameObject title;
	private float titleBaseY;
	private Vector3 titleBaseScale;
	private Text titleText, titleShadow, subTitle, subTitleShadow;
	private Text[] titleLogo;

	private Text scoreText, scoreShadow;

	private float tick;

	public static int TICKHITS = 0;
	private int hits = 0;

	public static int TICK = 0;
	public static int BAR {
		get {
			return (TICK % 16 - 1) == -1 ? 15 : (TICK % 16 - 1);
		}
	}

	public int BPM;
	public float bassVolume = 0.5f, melodyVolume = 0.5f;
	public float bassPitch = 0.99f;

	public static int bpm;
	private double nextTime, nextBeat;
	private double sampleRate = 0;
	private bool ticked = false, tickedBeat = false;

	private double currentDSP;

	private static BeatGenerator self;

	private AudioClip lastBassNote;

	[Header("Rhythm")]
	public AudioSource audio_tick;
	public AudioSource audio_kick, audio_hit, audio_hat, audio_loRide, audio_hiRide;

	[Header("Bass")]
	public AudioSource audio_bass_root;
	public AudioSource audio_bass_tonic, audio_bass_major, audio_bass_sad, audio_bass_fifth, audio_bass_5, audio_bass_4, audio_bass_7;

	[Header("Melody")]
	public AudioSource audio_mel_strum;
	public AudioSource audio_mel_third, audio_mel_minor, audio_mel_major, audio_mel_leading, audio_mel_sus;

	private int pulse = 0;
	private float beat = 0;
	private bool fastHats = false;
	private bool isLastNoteHit = false;

	private AudioSource[] all;

	void Awake () {
		all = new AudioSource[]{audio_kick, audio_hit, audio_bass_tonic, audio_bass_major, audio_bass_sad, audio_bass_fifth, audio_bass_5, audio_bass_4, audio_bass_7,
		audio_mel_strum, audio_mel_third, audio_mel_minor, audio_mel_major, audio_mel_leading, audio_mel_sus, audio_loRide, audio_hiRide, audio_hat};

		scoreText = score.GetComponent<Text>();
		scoreShadow = score.transform.GetChild(0).GetComponent<Text>();
		scoreBaseY = score.transform.localPosition.y;
		titleBaseY = title.transform.localPosition.y;
		titleBaseScale = title.transform.localScale;
		title.transform.localScale = Vector3.zero;
		titleText = title.GetComponent<Text>();
		titleShadow = title.transform.GetChild(0).GetComponent<Text>();
		subTitle = title.transform.GetChild(1).GetComponent<Text>();
		subTitleShadow = title.transform.GetChild(1).GetChild(0).GetComponent<Text>(); 
		titleLogo = new Text[]{titleText, titleShadow, subTitle, subTitleShadow};

		lastBassNote = audio_bass_root.clip;
		self = this;
		bpm = BPM;
		AudioSource[] bass = new AudioSource[]{audio_bass_root, audio_bass_tonic, audio_bass_major, audio_bass_sad, audio_bass_fifth, audio_bass_5, audio_bass_7, audio_bass_4};
		foreach(AudioSource src in bass) {
			src.volume = bassVolume;
			src.pitch = bassPitch;
		}
		AudioSource[] mel = new AudioSource[]{audio_mel_strum, audio_mel_third, audio_mel_minor, audio_mel_major, audio_mel_leading, audio_mel_sus};
		foreach(AudioSource src in mel) src.volume = melodyVolume;

		double startTime = currentDSP = AudioSettings.dspTime;
		sampleRate = AudioSettings.outputSampleRate;
		nextTime = startTime + (60.0 / BPM);
		nextBeat = startTime + (60.0 / (BPM * 4));

	}

	public static void SetHit() {
		if(!self.isLastNoteHit) self.hits++;
		self.isLastNoteHit = true;
		//self.scoreText.text = self.scoreShadow.text =  "Score: " + self.hits;
	}

	public static float GetBPM() {
		return (float)(60.0 / self.BPM);
	}

	void LateUpdate() {
		if(!ticked && nextTime >= AudioSettings.dspTime) {
			ticked = true;
			BroadcastMessage("OnPulse");
		}
		if(!tickedBeat && nextBeat >= AudioSettings.dspTime) {
			tickedBeat = true;
			OnTick();
		}
		foreach(AudioSource src in all) src.mute = !isLastNoteHit;
	}

	private void PlayMelody() {
		if(beat == 0) PlayNoteAt(audio_mel_strum.clip, melodyVolume, 1, true);
		else if(beat > 0 && ((beat % 8 == 0) || beat == 12)){ 
			AudioSource[] sources = new AudioSource[]{audio_mel_leading, audio_mel_major, audio_mel_minor, audio_mel_sus, audio_mel_third};
			AudioSource current = sources[Random.Range(0, sources.Length)];
			lastBassNote = current.clip;
			PlayNoteAt(current.clip, melodyVolume, 1, true);
		}
		PlayNoteAt(lastBassNote, bassVolume - 0.2f, 1.5f, true);
	}

	private void playRoot(int i = 0) {
		AudioSource current = audio_bass_root;
		if(i == 7) current = audio_bass_7;
		else if(i == 5) current = audio_bass_5;
		else if(i == 4) current = audio_bass_4;

		PlayNoteAt(current.clip, bassVolume, bassPitch);
		if(i != 4) lastBassNote = current.clip;
	}
	protected AudioSource PlayNoteAt(AudioClip clip, float volume = 1, float pitch = 1, bool ignore = false) {
		if(!isLastNoteHit && !ignore) return null;
		var temp = new GameObject("TempAudio");
		temp.transform.position = Camera.main.transform.position;
		var source = temp.AddComponent<AudioSource>();
		source.clip = clip;
		source.pitch = pitch;
		source.volume = volume;
		source.spatialBlend = 0;
		source.Play();
		Destroy(temp, clip.length);
		return source;
	}

	private void PlayBass() {
		if(beat == 0) {
			PlayNoteAt(audio_bass_tonic.clip, bassVolume, bassPitch);
			lastBassNote = audio_bass_root.clip;
		}
		if(beat == 1.5f || beat == 2) playRoot();

		if(beat == 4) PlayNoteAt(audio_bass_sad.clip, bassVolume, bassPitch);
		if(beat == 5.5f || beat == 6) playRoot();

		if(beat == 8) PlayNoteAt(audio_bass_fifth.clip, bassVolume, bassPitch);
		if(beat == 9.5f) playRoot(7);
		if(beat == 10) playRoot(5);

		if(beat == 12) PlayNoteAt(audio_bass_major.clip, bassVolume, bassPitch);
		if(beat == 13.5f) playRoot(5);
		if(beat == 14) playRoot(4);
	}

	void OnPulse() {
		fastHats = Random.Range(0, 4) < 2;

		//Rhythm / Drums
		audio_tick.PlayScheduled(AudioSettings.dspTime * (60.0 / BPM));
		if(pulse == 2) PlayNoteAt(audio_hit.clip);//audio_hit.PlayScheduled(AudioSettings.dspTime * (60.0 / BPM));
		if(pulse == 0) PlayNoteAt(audio_loRide.clip);
		else PlayNoteAt(audio_hiRide.clip, audio_hiRide.volume, Random.Range(0.85f, 1.1f));

		//Count of the rhythm
		if(pulse >= 3) pulse = 0;
		else pulse++;

		Level.Tick();
		TICK++;
		self.isLastNoteHit = false;
	}

	void OnTick() {
		PlayBass();
		PlayMelody();

		if(beat >= 15.75f) beat = 0;
		else beat += 0.25f;
		
		if(Random.Range(0, 10) < 3 && beat != 2f) audio_kick.Play();
		if(beat % 4 == 2 || beat % 4 == 1.5f) audio_kick.Play();

		if(pulse == 0 && (fastHats || (beat == 0 || beat == 0.5f))) audio_hat.Play();
	}

	void FixedUpdate() {
		if(title != null) {
			if(TICK > 5) {
				foreach(Text t in titleLogo) t.color = Color.Lerp(t.color, new Color(t.color.r, t.color.g, t.color.b, 0), Time.deltaTime * 2);
				if(TICK > 10) Destroy(title.gameObject);
			}
			else title.transform.localPosition = new Vector3(title.transform.localPosition.x, titleBaseY + Mathf.Sin(Time.time * 4) * 2, title.transform.localPosition.z);
			title.transform.localScale = Vector3.Lerp(title.transform.localScale, titleBaseScale, Time.deltaTime * 2);
		}

		//score.transform.localPosition = new Vector3(score.transform.localPosition.x, scoreBaseY + Mathf.Sin(Time.time * 4) * 2, score.transform.localPosition.z);

		double timePerBeat = 60.0 / (BPM * 4);
		double timePerTick = 60.0 / BPM;
		double dspTime = AudioSettings.dspTime;

		while(dspTime >= nextBeat) {
			tickedBeat = false;
			nextBeat += timePerBeat;
		}

		while(dspTime >= nextTime) {
			ticked = false;
			nextTime += timePerTick;
			currentDSP = AudioSettings.dspTime;
		}
	}

	public static double LastBeat() {
		return self.currentDSP;
	}
}
