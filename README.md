# LibSoxSharp

## Overview

LibSoxSharp is a .Net wrapper around the C libsox library for access to audio conversion / handling <br \>
It can be used for audio editing / transformations / conversions.

* Allows for converting of audio between different formats
* Allows for the chaining of audio effects

It should be noted that as part of this we build libsox out as a dll <br \>
and make calls to it via swig, libsox generally don't support this type of build themselves

## Code Layers

There are approx 3 layers to the code

* **The C Layer** - this includes libsox the main library <br \>
  plus any dependencies such as lame - mp3 encoding / mad - mp3 decoding
* **The Swig Layer** - this is auto generated C# code from a tool called swig, <br \>
  this joins together the .Net and C layers, and makes it easier (a lot easier) <br \>
  to access the underlying C Structures and functions
* **The LibSoxSharp Layer** - this is the library accessed at the .Net level to make calls to libsox

## TODO

 * Version sox-14.4.1 works but has a bunch of memory leaks in unmanaged memory, I think when loading / unloading the dll's within the .Net wrapper
   Ideally I need to re-do the swig bindings for sox-14.4.2 / re-generate the unmanaged dll's for this version to see if this solves the problem
 
 * Currently we're using the old Visual Studio Project files for building the unmanaged libsox dll's
   For better cross platform compatibility and for additional support of more libraries that can be built into libsox, we want to instead be generating
   these files via cmake. Not all libsox depends have cmake files for generating project files though currently.
 
 * Additional Apps needed for testing / examples
   Ideally we also need some NUnit test projects to check for memory leaks in the unmanaged code
 
 * I'd like to move to a build system for this project where the project files are generated via meta data for cross compatibility
   One idea is premake, but I'm currently more fond of the idea of using a combination of scriptcs, cmake, and a .Net library wrapper for CMake to generate
   the project files.

 * Additional Effect class's to be added in to match those effects inbuilt to libsox

 * Investigate the possibility of streaming in live audio, instead of using the file input / output effects
   try to see if we can setup custom streaming class's using the handler class's
