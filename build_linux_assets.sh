#!/bin/bash

mkdir OutputFiles
mkdir SetupLinux/AppImage/AppDir/usr
mkdir SetupLinux/AppImage/AppDir/usr/bin
cp Builds/linux-x64/* SetupLinux/AppImage/AppDir/usr/bin
chmod +x SetupLinux/AppImage/AppDir/usr/bin/SwagLyricsGUI
cat version.txt >> SetupLinux/AppImage/SwagLyricsGUI.desktop 
wget -P SetupLinux/AppImage/ "https://github.com/AppImage/AppImageKit/releases/download/continuous/appimagetool-x86_64.AppImage"
chmod a+x SetupLinux/AppImage/appimagetool-x86_64.AppImage
export ARCH=x86_64
./SetupLinux/AppImage/appimagetool-x86_64.AppImage ./SetupLinux/AppImage/AppDir/
mv SwagLyricsGUI*.AppImage OutputFiles/
chmod +x OutputFiles/SwagLyricsGUI-x86_64.AppImage
