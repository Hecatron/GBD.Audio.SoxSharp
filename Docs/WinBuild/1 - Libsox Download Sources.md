# Downloading the sources for LibSox

## Overview

In order to use libsox from a .Net wrapper within windows we first need to compile libsox out as a dll <br/>
Generally libsox don't support this as a rule but I've managed to get it to work with a bit of tweaking so far

This doc relates to version **sox-14.4.1** for downloading the sources

## Downloading the sources

First we need to download / extract the source for libsox


* Download LibSox from http://sox.sourceforge.net/
* Extract to a temporary directory using winzip
* Example: D:\Source\libsox

Next we need to extract the source code for the following packages <br/>
We also need to remove the vesion numbers from the end of the directory names <br/>

The layout should look something like this

* **D:\Source\libsox** <br/>
  [Version 14.4.1 - Main libsox library](http://sox.sourceforge.net/)
* **D:\Source\flac** <br/>
  [Version 1.2.1 - Flac Audio Support](http://sourceforge.net/projects/flac/files/flac-src/flac-1.2.1-src/)
* **D:\Source\lame** <br/>
  [Version 3.99.5 - Lame MP3 Encoding Support](http://sourceforge.net/projects/lame/files/lame/3.99/)
* **D:\Source\libid3tag** <br/>
  [Version 0.15.1b - ID3 Mp3 Tag Support](http://sourceforge.net/projects/mad/files/libid3tag/)
* **D:\Source\libmad** <br/>
  [Version 0.15.1b - Mad MP3 Decoding Support](http://sourceforge.net/projects/mad/files/libmad/0.15.1b/)
* **D:\Source\libogg** <br/>
  [Version 1.2.2 - Ogg Vorbis Support](http://downloads.xiph.org/releases/ogg/)
* **D:\Source\libpng** <br/>
  [Version 1.5.1 - Png Support](http://sourceforge.net/projects/libpng/files/libpng15/older-releases/1.5.1/)
* **D:\Source\libsndfile** <br/>
  [Version 1.0.23 - Snd File Support](http://www.mega-nerd.com/libsndfile/files/)
* **D:\Source\libvorbis** <br/>
  [Version 1.3.2 - Ogg Vorbis Support](http://downloads.xiph.org/releases/vorbis/)
* **D:\Source\speex** <br/>
  [Version 1.2rc1 - Speex Codec Support](http://www.speex.org/downloads/)
* **D:\Source\wavpack** [HomePage](http://www.wavpack.com/downloads.html) <br/>
  [Version 4.60.1 - Wavpack codec Support](http://mirrors.xmission.com/xbmc/build-deps/sources/wavpack-4.60.1.tar.bz2)
* **D:\Source\zlib** <br/>
  [Version 1.2.5 - ZLib Compression Support](http://sourceforge.net/projects/libpng/files/zlib/1.2.5/)

## Speex File

Next we need to download a speexdsp.c file
* http://sox.cvs.sourceforge.net/viewvc/sox/sox/src/

Place speexdsp.c into the **libsox\src** directory
