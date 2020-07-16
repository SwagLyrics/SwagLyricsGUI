[![Discord Server](https://badgen.net/badge/discord/join%20chat/7289DA?icon=discord)](https://discord.gg/DSUZGK4)

# SwagLyricsGUI

A cross-platform, real-time lyrics fetching application.

![screenshot](https://github.com/SwagLyrics/SwagLyricsGUI/blob/master/Images/guidark.png)

# About

SwagLyricsGUI is a GUI wrapper for super fast [SwagLyrics-For-Spotify](https://github.com/SwagLyrics/SwagLyrics-For-Spotify) library. It contains 3 themes and auto scroll.

# Installing 

Download appropriate file for your system [here](https://github.com/SwagLyrics/SwagLyricsGUI/releases/latest).

Make sure you have installed `Python 3.6+`, if not download it [here](https://www.python.org/downloads/)

## Windows

Follow setup and simply open SwagLyricsGUI application

## Linux

Currently pre-built binaries are wrapped in `AppImage`.

Running file should work out of the box, but sometimes you might get `Permissions denied`,
or `Could not display SwagLyricsGUI.AppImage`, in that case use `chmod +x SwagLyricsGUI-x86_64.AppImage`.

# Build from source

SwagLyricsGUI is a multilingual application. It means that it uses multiple programming languages in it's core, `C#` for controlling UI and `Python` for lyrics fetching backend. 

Application is written in AvaloniaUI framework.

## Prerequisites

- `.NET Core 3.1 SDK`
- Minimum `Python 3.6`
- Optionally download [SwagLyrics-For-Spotify](https://github.com/SwagLyrics/SwagLyrics-For-Spotify) library, but application will do that for you in first run.

## Building

Open .sln file in Visual Studio and hit Start or use CLI .NET Core `dotnet build` in root directory and `dotnet run` for selected project (Tests or GUI).

### Example for running GUI using dotnet

`dotnet build` in root git directory

`cd SwagLyricsGUI`

`dotnet run`


# Contributing

Feel free to contribute! We don't have any guide for this yet, but with application growth it might show up :) 
