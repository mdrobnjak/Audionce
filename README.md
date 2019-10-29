# Audionce
Music Visualizer

## Modules
Sound Capture
Fourier Transform
GUI
Auto Settings
Visuals

Save/Load
MIDI Input
Arduino LED Interface

### Sound Capture
This module uses 3rd party sound capture libraries to get the digital audio output stream from Windows.

```
L/R?
Buffer Size?
```

### Fourier Transform
This module applies a Fourier Transform to the digital audio stream in order to get frequency/amplitude values in real time with minimal latency.

```
Can module performance be improved? (Can latency be reduced or resolution be improved?)
Why FFT?
```

### Gate
This module defines the audio data features which will trigger visual effects in the Visuals module.

### GUI
This module defines how users interact with and configure gate parameters. Touch controls are supported.

### Auto Settings
This module automatically configures gate parameters to optimize music detection.

```
ML Features
```

### Visuals
This module defines the graphical output of the program, which includes animations triggered by the Gate module.

### Save/Load
This module defines the save/load options for configuration.

### MIDI Input
This module provides an interface between a MIDI data input and the Visuals module.

### Arduino LED Interface
This module provides an additional output from the Gate module triggers to an Arduino controlling LEDs.
