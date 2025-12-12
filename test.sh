#!/bin/bash
# Run difficulty simulation tests

echo -e "\033[36mRunning Challenge Mode Tests...\033[0m\n"

cd schedule-one-challenge-mode.Tests
dotnet test --logger "console;verbosity=detailed"

cd ..

