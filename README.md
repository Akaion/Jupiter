## Jupiter

A Windows virtual memory editing library with support for pattern scanning.

----

### Features

* Allocate memory
* Free memory
* Protect memory
* Read memory
* Write memory

### Pattern Scanning Features

* Support for wildcard bytes
* A Bower-Moore-Horspool algorithm implementation for fast scanning, particularly for large patterns

----

### Installation

* Download and install Jupiter using [NuGet](https://www.nuget.org/packages/Jupiter)

----

### Usage

The example below describes a basic implementation of the library

```csharp
using Jupiter;

using (var memoryModule = new MemoryModule(processName))
{
    // Allocate a region of virtual memory in the process

    var regionAddress = memoryModule.AllocateVirtualMemory(sizeof(int), MemoryProtection.ReadWrite);

    // Write a value into the newely allocated region

    memoryModule.WriteVirtualMemory(regionAddress, 25);

    // Scan for the value we just wrote into memory

    var patternAddresses = memoryModule.PatternScan(BitConverter.GetBytes(25));

    // Read the value back

    var value = memoryModule.ReadVirtualMemory<int>(regionAddress);

    // Free the region of virtual memory

    memoryModule.FreeVirtualMemory(regionAddress);
}

```

----

### Overloads

You can use a process ID instead of a process name.

```csharp
var memoryModule = new MemoryModule(processId)
```

You can specify an address in which you want to allocate a region of virtual memory.

```csharp
var regionAddress = memoryModule.AllocateVirtualMemory(pointer, sizeof(int), MemoryProtection.ReadWrite);
```

You can read an array of bytes instead of a structure.

```csharp
var bytes = memoryModule.ReadVirtualMemory(regionAddress, bytesToRead);
```

You can write an array of bytes instead of a structure.

```csharp
memoryModule.WriteVirtualMemory(regionAddress, new byte[] {0x19, 0xF0, 0x00, 0x2A});
```

You can use a string with wildcards as the pattern for pattern scanning.

```csharp
memoryModule.PatternScan("19 F0 ?? 2A");
```

You can specify a base address in which the pattern scanning should start at to allow quicker scans.

```csharp
memoryModule.PatternScan(pointer, BitConverter.GetBytes(25));
```

----

### Caveats

* If no memory protection constant is specified when allocating virtual memory, ExecuteReadWrite will be used.

* Pattern scanning with a small pattern may take a long time depending on the speed of your CPU.

----

### Contributing

Pull requests are welcome. 

For large changes, please open an issue first to discuss what you would like to contribute.
