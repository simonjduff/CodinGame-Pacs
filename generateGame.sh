#!/bin/bash
find src/ -name obj -prune -o -name bin -prune -o -name '*.cs' -exec cat {} \; > game.cs
