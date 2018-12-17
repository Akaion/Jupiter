# Jupiter

[![Build status](https://ci.appveyor.com/api/projects/status/jp6fnwbq34w012gj?svg=true)](https://ci.appveyor.com/project/Akaion/jupiter)

A Windows memory editing library written in C# that supports several memory editing methods.

## Supported Methods

* Allocate Memory
* Free Memory
* Read Memory
* Write Memory

## Extensions

* Pattern Scanning (AOB Scanning)

## Installation

* Download and install Jupiter using [NuGet](https://www.nuget.org/packages/Jupiter)

## Useage

You can also overload any method with a process id  or a `SafeHandle` to the process instead of a process name.

```csharp
using Jupiter;

var memoryModule = new MemoryModule();

// Allocate memory

var memoryAddress = memoryModule.AllocateMemory("processName", size);

// Free memory

memoryModule.FreeMemory("processName", address, size);

// Read a byte array from memory

var memoryBytes = memoryModule.ReadMemory("processName", address, size);

// Read a datatype or structure from memory

var memoryBoolean = memoryModule.ReadMemory<bool>("processName", address);

// Write a datatype or structure to memory

memoryModule.WriteMemory("processName", address, object);

// Find the address of a pattern

var patternAddress = memoryModule.PatternScan("processName", baseAddress, "45 FF ?? 01 ?? ?? 2A");
```

To read a string from memory you must do the following

```csharp
using System.Text;
using Jupiter;

var memoryModule = new MemoryModule();

// Read the bytes of the string

var memoryStringBytes = memoryModule.ReadMemory("processName", address, size);

// Convert the bytes to a string

var memoryString = Encoding.Default.GetString(memoryStringBytes);
```

## Contributing

Pull requests are welcome. 

For large changes, please open an issue first to discuss what you would like to add.
