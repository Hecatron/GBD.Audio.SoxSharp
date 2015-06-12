# Patching libsox for X64

## Overview

This is still kind of experimental, but I've managed to come up with a list of changes
to allow for x64 support. Be wary of memory leaks when loading / unloading the libsox dll
also when performing conversions of audio / running chains

## Setup the Solution

### Create a X64 Profile

First we're going to create a x64 profile within the Solution within visual Studio

* Open up the libsox solution file libsox/msvc10/Sox.sln (I'm using Visual Studio 2013 here)
* Right Click Solution -> Properties
* Select Configuration Properties -> Configuration Manager
* Select New Platform / X64
* Copy Settings from Win32

### Disable Warnings as Errors

In order to get things to work, I had to disable the "show warnings as Errors"
setting within some of the project files <br/>
To change this setting select

* Right click Project -> Properties -> Configuration Properties -> C/C++ -> General
* For "Treat Warnings as Errors" Select No

I needed to change this on the foilowing projects

* LibId3Tag
* LibMp3Lame
* LibSpeex
* LibVorbis
* LibWavPack
* LibFlac
* LibSndFile
* LibSox

## Patch the Sources

### Lame X64 Fix

TODO

### LibMad X64 Fix

TODO

### LibFlac X64 Fix

TODO

### LibSndfile Output path fix

TODO

### LibSndfile Missing Files

TODO
