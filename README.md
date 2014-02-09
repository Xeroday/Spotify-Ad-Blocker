EZBlocker
=========

EZBlocker is a Spotify Ad Blocker. It started as a program that I quickly put together with VB.NET. The latest version is written in C#.

It uses non-intrusive methods to read the Spotify client to extract song information. This information is then cross-checked with the iTunes and Spotify libraries to detect whether or not the song is an advertisement. When an advertisement is playing, EZBlocker will mute Spotify and automatically resume playing regular music when the ad is over.

Because EZBlocker does not modify the Spotify client in any ways, it is less buggy compared to other ad blockers. The goal for EZBlocker is to be the most reliable and stable ad blocker for Spotify.

For more info, visit the [EZBlocker project page][2].

To download a pre-compiled binary of the latest version, click [here][1].

Changelog (Major releases only):
 - V 1.3 (Feb 8, 2014):
  - Fix muting for some users
  - Fix crashing for adding to blocklist
  - Added Google Analytics
 - V 1.2.2 (Jan 24, 2014):
  - More improvements on ad detection (Many less false-positives)
  - Fix not muting when minimized bug
  - Cleaner ad detection code
  - Faster blocklist lookups
 - V 1.2 (Jan 9, 2014):
  - Port EZBlocker to C# (Still .NET 3.5)
  - Improve accuracy of blocklist checking
  - Improve accuracy of auto ad detection
  - Improve main muting logic
  - Minimize notification balloons
  - New option to disable notifications
 - V 1.1:
  - Add auto blacklist adding mechanism
  - Add update checker
  - Bug fixes

Additional Contributors:
 - [naspert][3]
 - [andreabresolin][5]
 - [pmorissette][4]
 - To contribute, just send a pull a request or raise an issue. Plase follow standard open-source contributing guidelines.


  [1]: http://www.ericzhang.me/dl/?file=EZBlocker.php
  [2]: http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/
  [3]: https://github.com/naspert
  [4]: https://github.com/pmorissette
  [5]: https://github.com/andreabresolin