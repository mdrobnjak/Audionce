using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public class PitchDetector
    {
        public PitchDetector()
        {
            for (int i = 0; i < windowSize; i++)
                timeVector.Add((double)i / sampleRate);
        }

        public class Note
        {
            /// <summary>
            /// Semitones from the base note
            /// </summary>
            public int Semitone;
            public double Frequency;

            public Note()
            { }

            public Note(int semitone, double freq)
            {
                Semitone = semitone;
                Frequency = freq;
            }

            public string Name => NoteFrequencies.GetNoteName(Semitone);

            public static bool operator ==(Note n1, Note n2) => n1?.Semitone == n2?.Semitone;
            public static bool operator !=(Note n1, Note n2) => !(n1 == n2);

            public override bool Equals(object obj) => base.Equals(obj);
            public override int GetHashCode() => base.GetHashCode();

            public override string ToString() => $"{Name} @ { Frequency }";

            public static Note BuildNote(string name, double frequency) => new Note(NoteFrequencies.GetSemitonesFromBase(name), frequency);

            public static Note Empty => new Note(0, 0);
        }

        public static class NoteFrequencies
        {
            private static double formulaBase = Math.Pow(2, 1.0 / 12);

            private static List<Note> stringNotes = new List<Note>();


            public static void GenerateNoteFrequencies(double freqA4)
            {
                //int indexA4 = Notes.FindIndex(n => n.Name == "A4");
                //for (int i = 0; i < Notes.Count; i++)
                //{
                //    Log.Info("", $"Current: {Notes[i].Frequency}, Calculated: {GetNoteFrequency(freqA4, i - indexA4)}");
                //    Notes[i] = new Note(Notes[i].Name, GetNoteFrequency(freqA4, i - indexA4));
                //}

                stringNotes.AddRange(new Note[]
                {
                Note.BuildNote("G3", 196),
                Note.BuildNote("D4", 293.66),
                Note.BuildNote("A4", 440.00),
                Note.BuildNote("E5", 659.25)
                });
            }

            public static Note GetNoteByFrequency(double freq)
            {
                Note note = stringNotes.Select(n => new Note(n.Semitone, freq - n.Frequency))
                                         .LastOrDefault(n => n.Frequency >= 0);
                if (note == null)
                    return Note.Empty;

                note.Frequency = GetNoteFrequency(stringNotes.Find(n => n == note).Frequency, 0);

                //Log.Info("Test", $"Core note {note}");

                var noteList = Enumerable.Range(0, 120)     // assume we have 120 notes
                                         .Select(i => new Note(i, GetNoteFrequency(note.Frequency, i - note.Semitone)));
                var minElement = noteList.Select(n => new Note(n.Semitone, Math.Abs(n.Frequency - freq)))
                                         .Aggregate((l, r) => Math.Abs(l.Frequency) < Math.Abs(r.Frequency) ? l : r);


                minElement.Frequency = GetNoteFrequency(note.Frequency, minElement.Semitone - note.Semitone);
                return minElement;
            }

            public static double GetNoteFrequency(double baseFreq, int n) => Math.Round(baseFreq * Math.Pow(formulaBase, n), 2);

            public static int GetSemitonesFromBase(string noteName)
            {
                var decomposedNote = DecomposeNoteName(noteName);

                int semitones = decomposedNote.Item1 + NoteNames.Count * decomposedNote.Item2;
                return semitones;
            }

            public static int GetNoteDistance(string note1, string note2)
            {
                return GetSemitonesFromBase(note1) - GetSemitonesFromBase(note2);
            }

            public static string GetNoteName(int semitonesFromBase)
            {
                int octave = semitonesFromBase / NoteNames.Count;
                int name = semitonesFromBase % NoteNames.Count;

                return $"{NoteNames[name]}{octave}";
            }

            private static (int, int) DecomposeNoteName(string noteName)
            {
                int name = NoteNames.FindIndex(n => n[0] == noteName[0]);
                int octave = -1;

                if (noteName.Length > 2)     // note has a # characteristic
                    name = NoteNames.FindIndex(n => n == noteName.Substring(0, 2));
                else
                    name = NoteNames.FindIndex(n => n == noteName.Substring(0, 1));

                int.TryParse(noteName.Last().ToString(), out octave);

                return (name, octave);
            }

            public static List<string> NoteNames = new List<string>()
            {
            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"
            };

            // frequencies for A4 = 440Hz
            //public static List<Note> Notes = new List<Note>()
            //{
            //    ("C0",  16.35),
            //    ("C#0", 17.32),
            //    ("D0",  18.35),
            //    ("D#0", 19.45),
            //    ("E0",  20.60),
            //    ("F0",  21.83),
            //    ("F#0", 23.12),
            //    ("G0",  24.50),
            //    ("G#0", 25.96),
            //    ("A0",  27.50),
            //    ("A#0", 29.14),
            //    ("B0",  30.87),
            //    ("C1",  32.70),
            //    ("C#1", 34.65),
            //    ("D1",  36.71),
            //    ("D#1", 38.89),
            //    ("E1",  41.20),
            //    ("F1",  43.65),
            //    ("F#1", 46.25),
            //    ("G1",  49.00),
            //    ("G#1", 51.91),
            //    ("A1",  55.00),
            //    ("A#1", 58.27),
            //    ("B1",  61.74),
            //    ("C2",  65.41),
            //    ("C#2", 69.30),
            //    ("D2",  73.42),
            //    ("D#2", 77.78),
            //    ("E2",  82.41),
            //    ("F2",  87.31),
            //    ("F#2", 92.50),
            //    ("G2",  98.00 ),
            //    ("G#2", 103.83),
            //    ("A2",  110.00),
            //    ("A#2", 116.54),
            //    ("B2",  123.47),
            //    ("C3",  130.81),
            //    ("C#3", 138.59),
            //    ("D3",  146.83),
            //    ("D#3", 155.56),
            //    ("E3",  164.81),
            //    ("F3",  174.61),
            //    ("F#3", 185.00),
            //    ("G3",  196.00),
            //    ("G#3", 207.65),
            //    ("A3",  220.00),
            //    ("A#3", 233.08),
            //    ("B3",  246.94),
            //    ("C4",  261.63),
            //    ("C#4", 277.18),
            //    ("D4",  293.66),
            //    ("D#4", 311.13),
            //    ("E4",  329.63),
            //    ("F4",  349.23),
            //    ("F#4", 369.99),
            //    ("G4",  392.00),
            //    ("G#4", 415.30),
            //    ("A4",  440.00),
            //    ("A#4", 466.16),
            //    ("B4",  493.88),
            //    ("C5",  523.25),
            //    ("C#5", 554.37),
            //    ("D5",  587.33),
            //    ("D#5", 622.25),
            //    ("E5",  659.25),
            //    ("F5",  698.46),
            //    ("F#5", 739.99),
            //    ("G5",  783.99),
            //    ("G#5", 830.61),
            //    ("A5",  880.00),
            //    ("A#5", 932.33),
            //    ("B5",  987.77 ),
            //    ("C6",  1046.50),
            //    ("C#6", 1108.73),
            //    ("D6",  1174.66),
            //    ("D#6", 1244.51),
            //    ("E6",  1318.51),
            //    ("F6",  1396.91),
            //    ("F#6", 1479.98),
            //    ("G6",  1567.98),
            //    ("G#6", 1661.22),
            //    ("A6",  1760.00),
            //    ("A#6", 1864.66),
            //    ("B6",  1975.53),
            //    ("C7",  2093.00),
            //    ("C#7", 2217.46),
            //    ("D7",  2349.32),
            //    ("D#7", 2489.02),
            //    ("E7",  2637.02),
            //    ("F7",  2793.83),
            //    ("F#7", 2959.96),
            //    ("G7",  3135.96),
            //    ("G#7", 3322.44),
            //    ("A7",  3520.00),
            //    ("A#7", 3729.31),
            //    ("B7",  3951.07),
            //    ("C8",  4186.01),
            //    ("C#8", 4434.92),
            //    ("D8",  4698.63),
            //    ("D#8", 4978.03),
            //    ("E8",  5274.04),
            //    ("F8",  5587.65),
            //    ("F#8", 5919.91),
            //    ("G8",  6271.93),
            //    ("G#8", 6644.88),
            //    ("A8",  7040.00),
            //    ("A#8", 7458.62),
            //    ("B8",  7902.13 )
            //};
        }

        const int windowSize = 2646;
        const int sampleRate = AudioIn.RATE;
        const double maxVal = 2;
        float threshold = 0.0f;
        int notesMemory = 3;
        int austerity = 6;

        private List<double> timeVector = new List<double>(windowSize);

        private List<(Note, double)> notesPlayed = new List<(Note, double)>();

        double DetectPitch(double[] buffer)
        {
            Complex[] input = new Complex[buffer.Length];

            // TODO: Add gauss window and check out frequency leaking
            // TODO: Investigate filter
            var filter = MathNet.Filtering.IIR.OnlineIirFilter.CreateLowpass(MathNet.Filtering.ImpulseResponse.Finite, sampleRate, 1000, 100);
            input = filter.ProcessSamples(buffer).Select(o => new Complex(o, 0)).ToArray();

            double[] g_window = MathNet.Numerics.Window.Gauss(windowSize, 1);

            if (input.Length != windowSize)
            {
                Console.WriteLine("Handle Buffer size", "input buffer not of size window");
                return 0;
            }

            // FFT input buffer and get the right frequencies
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(input);
            double[] freqs = MathNet.Numerics.IntegralTransforms.Fourier.FrequencyScale(windowSize, sampleRate);

            // fourier threshold to reject noise
            if (input.Max(i => i.Real) < threshold)
                return 0;

            // calculate autocorrelation
            Complex[] autocor = input.Select(i => i * Complex.Conjugate(i)).ToArray();

            var temp = (from c in input
                        orderby c.Magnitude descending
                        select c).Take(3).ToList();

            Console.WriteLine("Fourier", $"Fourier maximum at: {freqs[input.ToList().FindIndex(i => i == temp[0])]}, {temp[0].Magnitude} {freqs[input.ToList().FindIndex(i => i == temp[1])]}, {temp[1].Magnitude} {freqs[input.ToList().FindIndex(i => i == temp[2])]}, {temp[2].Magnitude}");

            // return to time domain
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(autocor);

            // cap autocorrelation values and strengthen early peaks
            double[] result = new double[windowSize];

            for (int i = 0; i < autocor.Length; i++)
            {
                if (i <= 20)
                    result[i] = 0;
                else
                    result[i] = autocor[i].Real *
                                        (maxVal - (1 - maxVal) / (timeVector[0] - timeVector.Last()) * (timeVector[i] - timeVector[0]));
            }

            var cortemp = (from r in result
                           orderby r descending
                           select r).Take(2).ToList();

            Console.WriteLine("Correlation", $"Correlation maximum at: {1.0 / timeVector[result.ToList().FindIndex(i => i == cortemp[0])]}, {cortemp[0]} {1.0 / timeVector[result.ToList().FindIndex(i => i == cortemp[1])]}, {cortemp[1]}");

            // calculate autocorrelation maximum
            int max = 0;
            for (int i = 0; i < result.Length / 2 + 1; i++)
                if (result[i] > result[max])
                    max = i;

            if (max - 1 < 0 || max + 1 >= result.Length)
                return 0;

            double[] p3 = { 1.0 / timeVector[max - 1], result[max - 1] };
            double[] p2 = { 1.0 / timeVector[max], result[max] };
            double[] p1 = { 1.0 / timeVector[max + 1], result[max + 1] };

            double max_freq = QuadraticMaximum(p1, p2, p3);
            Console.WriteLine(max_freq);
            return max_freq;
        }

        public void HandleAudioData()
        {
            float[] temp = SoundCapture.GetBuffer();
            double[] buffer = temp.Select(i => (double)i).ToArray();

            var median = MathNet.Filtering.Median.OnlineMedianFilter.CreateDenoise(7);
            buffer = median.ProcessSamples(buffer);

            double detectedFrequency = DetectPitch(buffer);
            Note noteEstimation;

            if (detectedFrequency > 0 && detectedFrequency < 2000)
                noteEstimation = NoteFrequencies.GetNoteByFrequency(detectedFrequency);
            else
                noteEstimation = Note.Empty;

            double difference = detectedFrequency - noteEstimation.Frequency;

            string characteristic = "OK";
            if (noteEstimation.Frequency == 0)
            {
                ClearNotes();
                return;
            }

            Note nextNote = new Note(difference > 0 ? noteEstimation.Semitone + 1 : noteEstimation.Semitone - 1,
                                             NoteFrequencies.GetNoteFrequency(noteEstimation.Frequency, 1 * Math.Sign(difference)));

            double threshold = Math.Sign(difference) * (nextNote.Frequency - noteEstimation.Frequency) / austerity;
            double successPercent = (difference / Math.Abs((nextNote.Frequency - noteEstimation.Frequency)) * 100);

            if (difference > 0)
                if (detectedFrequency > noteEstimation.Frequency + threshold)           // played note frequency is above the limit.
                    characteristic = "+";
                else
                if (detectedFrequency < noteEstimation.Frequency - threshold)           // played note is below the limit
                    characteristic = "-";

            // when the note vector reaches the given memory size, calculate median and print results
            if (AppendNote((noteEstimation, detectedFrequency)))
            {
                double meanFreq = GetNotesMedian();
                ClearNotes();
                Console.WriteLine($"Note detected {noteEstimation.Name} {meanFreq.ToString("N1")}.\n {characteristic}.\nDifference {successPercent.ToString("N3")}%");
            }
        }

        double QuadraticMaximum(double[] p1, double[] p2, double[] p3)
        {
            double a = p1[1] / ((p1[0] - p2[0]) * (p1[0] - p3[0])) + p2[1] / ((p2[0] - p1[0]) * (p2[0] - p3[0])) + p3[1] / ((p3[0] - p1[0]) * (p3[0] - p2[0]));
            if (a == 0)
                return 0;
            double b = -p1[1] * (p2[0] + p3[0]) / ((p1[0] - p2[0]) * (p1[0] - p3[0])) - p2[1] * (p3[0] + p1[0]) / ((p2[0] - p1[0]) * (p2[0] - p3[0])) - p3[1] * (p1[0] + p2[0]) / ((p3[0] - p1[0]) * (p3[0] - p2[0]));
            double c = p1[1] * p2[0] * p3[0] / ((p1[0] - p2[0]) * (p1[0] - p3[0])) + p2[1] * p3[0] * p1[0] / ((p2[0] - p1[0]) * (p2[0] - p3[0])) + p3[1] * p1[0] * p2[0] / ((p3[0] - p1[0]) * (p3[0] - p2[0]));
            double max_val = -b / (2 * a);
            return max_val;
        }

        bool AppendNote((Note, double) n)
        {
            notesPlayed.Add(n);

            if (notesPlayed.Count > notesMemory)
                return true;

            return false;
        }

        void ClearNotes()
        {
            notesPlayed.Clear();
        }

        double GetNotesMedian()
        {
            notesPlayed.Sort((n1, n2) => n1.Item2 > n2.Item2 ? 1 : -1);

            if (notesMemory % 2 == 0)
                return (notesPlayed[notesMemory / 2].Item2 + notesPlayed[notesMemory / 2 + 1].Item2) / 2;
            else
                return notesPlayed[notesPlayed.Count / 2 + 1].Item2;
        }
    }
}
