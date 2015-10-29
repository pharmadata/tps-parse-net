# TPS Parser for .NET

Library for parsing Clarion TPS files

Licensed under the [Apache 2 License](https://www.apache.org/licenses/LICENSE-2.0.html)

WARNING : This software is based on Reverse Engineered TPS Files. As such, its probably incomplete and may mis-interpret data. It is no replacement for any existing Clarion tooling. Check the output files thoroughly before proceeding.

This is a loose port of [tps-parse](https://github.com/ctrl-alt-dev/tps-parse/) for Java.  Many, many thanks to [Erik Hooijmeijer](https://blog.42.nl/articles/author/erik-hooijmeijer/) for his thorough analysis and implementaion
of the file format.

Some refactoring/redesign has been done compared to the Java version to match the .NET coding style better.

The main library is written in .NET 2.0 to allow for better compatibility with older projects.

Read the [blogpost](http://blog.42.nl/articles/liberating-data-from-clarion-tps-files) for more information from Erik about the file format.

## Usage

```cs
using (
    var file = File.Open(@"MYFILE.TPS", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
{
    var tps = new TpsFile(file);
    foreach (var definition in tps.GetTableDefinitions())
    {
        foreach (var field in definition.Fields)
        {
            // Do something with the field
        }
        foreach (var record in tps.GetDataRecords(definition))
        {
            // Do something with the record
        }
    }
}
```

## Samples

The source includes a samples project:

* **Export to CSV** - shows how to export a TPS file to CSV (only works for TPS files with a single table)
* **Print Schema** - prints the basic schema of a TPS file

## Roadmap

* **Unit tests** - need tests to ensure high quality
* **Encrypted files** - needs to be ported from Java
* **Index lookups** - should allow the use of indexes to lookup records

## Version history

### 1.0 (October 29, 2015)

Initial release
