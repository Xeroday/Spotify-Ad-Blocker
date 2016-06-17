SpotifyMuter
============

[![License](https://img.shields.io/github/license/Maschmi/SpotifyMuter.svg)](https://github.com/Maschmi/SpotifyMuter/blob/master/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/su69kmy9ma8csmma/branch/master?svg=true)](https://ci.appveyor.com/project/Maschmi/spotifymuter/branch/master)
[![codecov](https://codecov.io/gh/Maschmi/SpotifyMuter/branch/master/graph/badge.svg)](https://codecov.io/gh/Maschmi/SpotifyMuter)

This is a fork of [EZBlocker](https://github.com/Xeroday/Spotify-Ad-Blocker).

It should work with Windows 7/8/10.

It uses non-intrusive methods to read the Spotify client to extract song information. When an advertisement is playing, SpotifyMuter will mute Spotify and automatically resume playing music when the ad is over.

The goal of this fork was to make it easier to use than EZBlocker.

Usage:

1. Start Spotify
2. Enable 'Allow Spotify to be opened from the web' (Edit -> Preferences... -> Show advanced settings)
2. Start SpotifyMuter, it will be minimized to tray.
3. Listen to music with Spotify and ads will be muted.

To close SpotifyMuter double click the trayicon.