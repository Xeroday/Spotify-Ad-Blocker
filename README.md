

EZBlocker
=========

EZBlocker is a Spotify Ad Blocker written in C# for Windows 7/8/10. The goal for EZBlocker is to be the most reliable ad blocker for Spotify.

When an advertisement is playing, EZBlocker will mute Spotify until it's over.

To download a pre-compiled binary of the latest version, click [here][1]. For more info, visit the [EZBlocker project page][2].

## Technical overview

The current version of EZBlocker hooks Spotify in three ways: window titles, audio sessions, and a reverse listener. 

**Window title**

The window title is used to grab the name of the currently playing song/artist/advertisement. EZBlocker also uses the window title to grab the Spotify process handle.

**Audio session**

Using lower level COM interfaces, EZBlocker is able to both find and extract information from Spotify's audio session. 

Firstly, if the Spotify window is hidden (in the tray), its window title cannot be used locate the correct Spotify process handle. In this case, EZBlocker falls back to searching through the audio sessions to find the correct process.

Secondly, the audio session is a somewhat reliable way to detect whether or not a song/advertisement is playing regardless of whether or not the Spotify window is hidden. It can be inaccurate at times, eg. when a song has a 3 second gap of no sound, but can automatically recover.

**Reverse Listener**

I've historically tried to avoid modifying the Spotify application, but since the shutdown of its unofficial local API (in mid July 2018), there was no reliable way to detect if an advertisement was playing.

Spotify is built with the Chromium Embedded Framework, which means many of its components are written in HTML/JS. EZBlocker patches one of them to attach a web worker that sends a signal to a local listener when an advertisement is playing.

More data could probably be extracted through the web worker, but I haven't had time to explore.


## Changelog (Major releases only):

- V 1.7 (July 22, 2018):
  - Almost a complete re-write of the application (lighter, more performant, cleaner code)
  - New Spotify ad detection and muting logic after Spotify's shutdown of its local API
- V 1.6 (March 10, 2016):
  - Better handling of Spotify updates/restarts
  - Fix bugs caused by newer Spotify local API
  - Update dependencies
  - Now requires .NET Framework 4.5

## Translations
To better support non-English speakers, I've started an effort to translate EZBlocker. Please reach out if you are a native speaker of a non-English language. 

The following are contributors to this goal:
- Portuguese: Raí

  [1]: http://www.ericzhang.me/dl/?file=EZBlocker.php
  [2]: http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/
