# ElgatoStreamDeck

Open Source and Cross Platform library for accessing Elgato Stream Deck devices through HidApi.
<br>
The original code written by [TheJebForge](https://github.com/TheJebForge), original version written in Rust by
[TheJebForge](https://github.com/TheJebForge).  Code originally written for
[StreamDuck](https://github.com/streamduck-org/streamduck).<br>

## Written with

Code written with HidApi.NET, and SixLabors.ImageSharp for loading of images.

## Installation

You can install this package through Nuget, by searching in your IDE for ElgatoStreamDeck, or by using
the command line:

```bash
dotnet add package ElgatoStreamDeck
```

Code should run anywhere HidApi runs.

## Extra Configuration

In order for ElgatoStreamDeck / HidApi to access HID Devices on Linux, a UDev rules file needs to be installed.
Copy the file 40-steamdeck.rules to /etc/udev/rules.d/ and restart your system.