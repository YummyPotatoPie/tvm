# tvm

![TinyVMLogo](readmeresourses/logo.png)

# TBC to CIL 
## Start of program 
### TBC 
``` TBC 
  # No commands
```
### CIL 
``` CIL 
  ldc.i4 0
  stloc.0 // r1 
  ldc.i4 0
  stloc.1 // r2
  ldc.i4 0
  stloc.2 // stack pointer
  ldc.i4 1024
  newarr [mscorlib]System.Int32
  stloc.3 // stack
```
## Push instruction 
### TBC 
``` TBC 
  push <value>
```
### CIL 
``` CIL 
  ldloc.2
  ldc.i4 1
  add 
  
```
