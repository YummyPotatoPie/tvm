# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
- Just-in-Time compiler for x86 architecture
- More commands for virtual machine

## [0.1.1] - 2021-09-24
## Added
- Registers: r1, r2, r3, r4
- Command "dup": copy value from stack top and push it
- Command "peek": copy value from stack top and set it to the register
- Command "preg": copy value from register and push it to the stack
- Added comments support for .tbc programs

## [0.1.0] - 2021-09-23
### Added 
- Simple interpreter for execute byte code
- Simple command: push, pop, add, sub, mul, div
- Byte code parser and compiler
- Special utility for compile .tbc files to .tbcc 