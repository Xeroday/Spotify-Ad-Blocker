
# Novaki92-Custom Edits/Fixes


This is my own personal spin on EZBlocker by Xeroday.

To download a pre-compiled binary of the latest version, click [here][3].

Changelog (Major releases):
 - V 1.8 (December 17, 2017)
  - Lowered exit tolerance time to close EZBlocker faster
  - Disabled "Spotify not found" notification
  - Disabled EZBlocker updates leading to Xeroday master
  - Disabled exit notification MessageBox
  - Updated WebHelperHook to match Xeroday master updates to regain JSON fetching
  - Updated latest Executable


 - V 1.7 (October 21, 2016)
  - Diabled 'Private Session' Warning Message
  - Disabled 'EZBlocker is Hidden' Notification
  - Added Album and Song labels
  - 'Report Problem' label links to this repos 'Issues' page
  - Labels rearranged and now crop to screen
  - Fixed Icon issue after opening from system tray
  - Fixed Label issue where artist/album/song label wouldn't change



# EZBlocker


EZBlocker is a Spotify Ad Blocker written in C# for Windows 7/8/10.

It uses non-intrusive methods to read the Spotify client to extract song information. When an advertisement is playing, EZBlocker will mute Spotify and automatically resume playing regular music when the ad is over.

Because EZBlocker does not modify the Spotify client in any way, it is less buggy compared to other ad blockers. The goal for EZBlocker is to be the most reliable and stable ad blocker for Spotify.

For more info, visit the [EZBlocker project page][2].

To download a pre-compiled binary of the latest version, click [here][1].

Changelog (Major releases only):
 - V 1.6 (March 10, 2016):
  - Better handling of Spotify updates/restarts
  - Fix bugs caused by newer Spotify local API
  - Update dependencies
  - Now requires .NET Framework 4.5
 - V 1.5 (March 6, 2015):
  - Support for Spotify versions 1.0 and above
  - Support for blocking banner advertisements and muting video advertisements
  - Adblocking is now fully automated and not dependent on any blocklists
  - UI revamped from user feedback
 - V 1.4 (November 22, 2014):
  - Huge improvements to ad-detection
  - More stable muting/unmuting of Spotify process
  - Minor bug fixes
 - V 1.3.6 (April 5, 2014):
  - Fix for new Spotify update (sneaky update broke most ad blockers)
  - Settings are now auto-saved
  - Option to mute whole computer for those w/ issues muting just Spotify
  - Blocklist now opens in an easier to use form
  - Better support for 64-bit systems
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


  [1]: http://www.ericzhang.me/dl/?file=EZBlocker.php
  [2]: http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/
  [3]: https://github.com/Novaki92/Spotify-Ad-Blocker/raw/master/Latest%20Executable/EZBlocker.exe
