# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
- Just-in-Time compiler for x86 architecture

## [0.8.1] - 2021-10-05
## Added
- Method StackMemory.Reserve(int size) that move top stack pointer 

## Changed
- Command "rstk" implementation
- Interruptions set

## [0.8.0] - 2021-10-05
## Added
- Command "adsr": read value from address
- Command "adsw": write vlue to the address
- Command "rstk": reserve stack 

## Fixed
- ByteCodeParser.Reset() error when Line does not sets to 1

## [0.7.0] - 2021-09-30
## Added
- Command "call" implementation
- Command "ret" implementation
- Command "mod": calculate remainder
- Command "int": implementation

## Changed 
- Now virtual machine have only 2 registers 

## [0.6.0] - 2021-09-29
## Added
- Command jumps commands
- Command "label" implementation
- Command "call" (not implemented yet)
- Command "ptrfw": pointer offset write

## Changed
- ByteCodeCompiler implementation, now it has two phases of compilation
- ByteCodeParser implementation, now it can reset command stream pointer
- Command "ptrfc" renamed, now its "ptrfr": pointer offset read

## [0.5.0] - 2021-09-28
## Added
- Command "xor": binary xor operation
- Command "or": binary or operatrion
- Command "and": binary and operation

## [0.4.0] - 2021-09-27
## Added
- Command "cmp": compares values and set special flag represents result of compare
- Flags for compare result
- Command "shiftr": right binary shift
- Command "shiftl": left binary shift

## [0.3.0] - 2021-09-26
## Added 
- Command "ptrfc": copy value from stack with offset of pointer
- Command "vpop": pop values from stack n - times
- Command "int": interruption (without implementation yet)

## Fixed 
- ByteCodeParser.SkipComments() some bug

## [0.2.0] - 2021-09-24
## Added
- Registers: r1, r2, r3, r4
- Command "dup": copy value from stack top and push it
- Command "peek": copy value from stack top and set it to the register
- Command "preg": copy value from register and push it to the stack
- Added comments support for .tbc programs

## [0.1.0] - 2021-09-23
### Added 
- Simple interpreter for execute byte code
- Simple command: "push", "pop", "add", "sub", "mul", "div"
- Byte code parser and compiler
- Special utility for compile .tbc files to .tbcc 
