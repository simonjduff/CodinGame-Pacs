#!/bin/bash
find . -type d \( -path pacman/obj -o -path pacman/bin \) -prune -o -name '*.cs' -exec cat {} \; > game.cs
