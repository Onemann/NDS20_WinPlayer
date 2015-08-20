//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibVlcWrapper
{
   public enum LibvlcStateT
   {
      LibvlcNothingSpecial = 0,
      LibvlcOpening,
      LibvlcBuffering,
      LibvlcPlaying,
      LibvlcPaused,
      LibvlcStopped,
      LibvlcEnded,
      LibvlcError
   }

   public enum LibvlcLogMessateTSeverity
   {
      Info = 0,
      Err = 1,
      Warn = 2,
      Dbg = 3
   }

   public enum LibvlcLogLevel
   {
       LibvlcDebug = 0,   /* *< Debug message */
       LibvlcNotice = 2,  /* *< Important informational message */
       LibvlcWarning = 3, /* *< Warning (potential error) message */
       LibvlcError = 4    /* *< Error message */
   }

   public enum LibvlcEventE
   {
      LibvlcMediaMetaChanged = 0,
      LibvlcMediaSubItemAdded,
      LibvlcMediaDurationChanged,
      LibvlcMediaParsedChanged,
      LibvlcMediaFreed,
      LibvlcMediaStateChanged,

      LibvlcMediaPlayerMediaChanged = 0x100,
      LibvlcMediaPlayerNothingSpecial,
      LibvlcMediaPlayerOpening,
      LibvlcMediaPlayerBuffering,
      LibvlcMediaPlayerPlaying,
      LibvlcMediaPlayerPaused,
      LibvlcMediaPlayerStopped,
      LibvlcMediaPlayerForward,
      LibvlcMediaPlayerBackward,
      LibvlcMediaPlayerEndReached,
      LibvlcMediaPlayerEncounteredError,
      LibvlcMediaPlayerTimeChanged,
      LibvlcMediaPlayerPositionChanged,
      LibvlcMediaPlayerSeekableChanged,
      LibvlcMediaPlayerPausableChanged,
      LibvlcMediaPlayerTitleChanged,
      LibvlcMediaPlayerSnapshotTaken,
      LibvlcMediaPlayerLengthChanged,

      LibvlcMediaListItemAdded = 0x200,
      LibvlcMediaListWillAddItem,
      LibvlcMediaListItemDeleted,
      LibvlcMediaListWillDeleteItem,

      LibvlcMediaListViewItemAdded = 0x300,
      LibvlcMediaListViewWillAddItem,
      LibvlcMediaListViewItemDeleted,
      LibvlcMediaListViewWillDeleteItem,

      LibvlcMediaListPlayerPlayed = 0x400,
      LibvlcMediaListPlayerNextItemSet,
      LibvlcMediaListPlayerStopped,

      LibvlcMediaDiscovererStarted = 0x500,
      LibvlcMediaDiscovererEnded,

      LibvlcVlmMediaAdded = 0x600,
      LibvlcVlmMediaRemoved,
      LibvlcVlmMediaChanged,
      LibvlcVlmMediaInstanceStarted,
      LibvlcVlmMediaInstanceStopped,
      LibvlcVlmMediaInstanceStatusInit,
      LibvlcVlmMediaInstanceStatusOpening,
      LibvlcVlmMediaInstanceStatusPlaying,
      LibvlcVlmMediaInstanceStatusPause,
      LibvlcVlmMediaInstanceStatusEnd,
      LibvlcVlmMediaInstanceStatusError,
   }

   public enum LibvlcPlaybackModeT
   {
      LibvlcPlaybackModeDefault,
      LibvlcPlaybackModeLoop,
      LibvlcPlaybackModeRepeat
   }

   public enum LibvlcMetaT
   {
      LibvlcMetaTitle,
      LibvlcMetaArtist,
      LibvlcMetaGenre,
      LibvlcMetaCopyright,
      LibvlcMetaAlbum,
      LibvlcMetaTrackNumber,
      LibvlcMetaDescription,
      LibvlcMetaRating,
      LibvlcMetaDate,
      LibvlcMetaSetting,
      LibvlcMetaUrl,
      LibvlcMetaLanguage,
      LibvlcMetaNowPlaying,
      LibvlcMetaPublisher,
      LibvlcMetaEncodedBy,
      LibvlcMetaArtworkUrl,
      LibvlcMetaTrackId
   }

   public enum LibvlcTrackTypeT
   {
      LibvlcTrackUnknown = -1,
      LibvlcTrackAudio = 0,
      LibvlcTrackVideo = 1,
      LibvlcTrackText = 2,
   }

   public enum LibvlcVideoMarqueeOptionT
   {
      LibvlcMarqueeEnable = 0,

      /// <summary>
      /// Marquee text to display.
      /// (Available format strings:
      /// Time related: %Y = year, %m = month, %d = day, %H = hour,
      /// %M = minute, %S = second, ... 
      /// Meta data related: $a = artist, $b = album, $c = copyright,
      /// $d = description, $e = encoded by, $g = genre,
      /// $l = language, $n = track num, $p = now playing,
      /// $r = rating, $s = subtitles language, $t = title,
      /// $u = url, $A = date,
      /// $B = audio bitrate (in kb/s), $C = chapter,
      /// $D = duration, $F = full name with path, $I = title,
      /// $L = time left,
      /// $N = name, $O = audio language, $P = position (in %), $R = rate,
      /// $S = audio sample rate (in kHz),
      /// $T = time, $U = publisher, $V = volume, $_ = new line) 
      /// </summary>
      LibvlcMarqueeText,

      /// <summary>
      /// Color of the text that will be rendered on 
      /// the video. This must be an hexadecimal (like HTML colors). The first two
      /// chars are for red, then green, then blue. #000000 = black, #FF0000 = red,
      ///  #00FF00 = green, #FFFF00 = yellow (red + green), #FFFFFF = white
      /// </summary>
      LibvlcMarqueeColor,

      /// <summary>
      /// Opacity (inverse of transparency) of overlayed text. 0 = transparent, 255 = totally opaque. 
      /// </summary>
      LibvlcMarqueeOpacity,

      /// <summary>
      /// You can enforce the marquee position on the video.
      /// </summary>
      LibvlcMarqueePosition,

      /// <summary>
      /// Number of milliseconds between string updates. This is mainly useful when using meta data or time format string sequences.
      /// </summary>
      LibvlcMarqueeRefresh,

      /// <summary>
      /// Font size, in pixels. Default is -1 (use default font size).
      /// </summary>
      LibvlcMarqueeSize,

      /// <summary>
      /// Number of milliseconds the marquee must remain displayed. Default value is 0 (remains forever).
      /// </summary>
      LibvlcMarqueeTimeout,

      /// <summary>
      /// X offset, from the left screen edge.
      /// </summary>
      LibvlcMarqueeX,

      /// <summary>
      /// Y offset, down from the top.
      /// </summary>
      LibvlcMarqueeY
   }

   public enum LibvlcVideoLogoOptionT
   {
      LibvlcLogoEnable,

      /// <summary>
      /// Full path of the image files to use.
      /// </summary>
      LibvlcLogoFile,

      /// <summary>
      /// X coordinate of the logo. You can move the logo by left-clicking it.
      /// </summary>
      LibvlcLogoX,

      /// <summary>
      /// Y coordinate of the logo. You can move the logo by left-clicking it.
      /// </summary>
      LibvlcLogoY,

      /// <summary>
      /// Individual image display time of 0 - 60000 ms.
      /// </summary>
      LibvlcLogoDelay,

      /// <summary>
      /// Number of loops for the logo animation. -1 = continuous, 0 = disabled.
      /// </summary>
      LibvlcLogoRepeat,

      /// <summary>
      /// Logo opacity value (from 0 for full transparency to 255 for full opacity).
      /// </summary>
      LibvlcLogoOpacity,

      /// <summary>
      /// Logo position
      /// </summary>
      LibvlcLogoPosition,
   }

   public enum LibvlcVideoAdjustOptionT
   {
      LibvlcAdjustEnable = 0,
      LibvlcAdjustContrast,
      LibvlcAdjustBrightness,
      LibvlcAdjustHue,
      LibvlcAdjustSaturation,
      LibvlcAdjustGamma,
   }

   public enum LibvlcAudioOutputDeviceTypesT
   {
      LibvlcAudioOutputDeviceError = -1,
      LibvlcAudioOutputDeviceMono = 1,
      LibvlcAudioOutputDeviceStereo = 2,
      LibvlcAudioOutputDevice_2F2R = 4,
      LibvlcAudioOutputDevice_3F2R = 5,
      LibvlcAudioOutputDevice51 = 6,
      LibvlcAudioOutputDevice61 = 7,
      LibvlcAudioOutputDevice71 = 8,
      LibvlcAudioOutputDeviceSpdif = 10
   }

   public enum LibvlcAudioOutputChannelT
   {
      LibvlcAudioChannelError = -1,
      LibvlcAudioChannelStereo = 1,
      LibvlcAudioChannelRStereo = 2,
      LibvlcAudioChannelLeft = 3,
      LibvlcAudioChannelRight = 4,
      LibvlcAudioChannelDolbys = 5
   }

   public enum LibvlcNavigateModeT
   {
       LibvlcNavigateActivate = 0,
       LibvlcNavigateUp,
       LibvlcNavigateDown,
       LibvlcNavigateLeft,
       LibvlcNavigateRight
   }
}
