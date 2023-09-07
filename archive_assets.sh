#!/bin/sh
find Assets \
-name "*.jpg" -o \
-name "*.jpeg" -o \
-name "*.png" -o \
-name "*.gif" -o \
-name "*.psd" -o \
-name "*.ai" -o \
-name "*.mp3" -o \
-name "*.wav" -o \
-name "*.ogg" -o \
-name "*.fbx" -o \
-name "*.blend" -o \
-name "*.obj" -o \
-name "*.a" -o \
-name "*.exr" -o \
-name "*.tga" -o \
-name "*.pdf" -o \
-name "*.zip" -o \
-name "*.dll" -o \
-name "*.unitypackage" -o \
-name "*.aif" -o \
-name "*.ttf" -o \
-name "*.rns" -o \
-name "*.reason" -o \
-name "*.lxo" | tar -czvf Assets.tar.gz -T -
