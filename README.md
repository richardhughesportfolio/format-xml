# format-xml

A command line utility to help format XML from `stdin` to `stdout`.

- [format-xml](#format-xml)
  - [Build Statuses](#build-statuses)
  - [Usage](#usage)
    - [Examples](#examples)
  - [Building](#building)
    - [Version](#version)
  - [License](#license)

## Build Statuses

![Build Mac](https://github.com/richardjhughes/format-xml/actions/workflows/build_windows.yml/badge.svg)
![Build Mac](https://github.com/richardjhughes/format-xml/actions/workflows/build_mac.yml/badge.svg)
![Build Mac](https://github.com/richardjhughes/format-xml/actions/workflows/build_linux.yml/badge.svg)

## Usage

The utility is called `fxml`. It takes XML via `stdin` and outputs the formatted XML via `stdout`. Errors are written to `stderr`.

Empty input results in empty output.

If the XML is invalid or cannot be formatted, an error will be written to `stderr` and the input will be written to `stdout`.

If an error occurs, `fxml` will still return `0`. If you would like a non-zero value returned, you can pass the `--strict` flag.

### Examples

Basic usage:

```bash
cat file.xml | fxml > formatted.xml
echo "<tag/>" | fxml
```

To return a non-zero value on error:

```bash
cat invalid.xml | fxml --strict > formatted.xml
```

Note that `formatted.xml` will still contain the contents of `invalid.xml`, because on error, the contents of `stdin` are outputted to `stdout`.

To see the help text:

```bash
fxml --help
```

## Building

Make sure you have the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download) installed.

To build, simply run the `build.py` script. This will build, test and package the application. The resulting package will be saved in `./packages/fxml.tar.gz`.

To build manually, you can do this from this repository's root directory:

```bash
dotnet build ./src/
dotnet test ./src/ --no-build
dotnet publish ./src/FormatXML/ --configuration="release" --output="./src/build/" --self-contained true
```

This will create a binary in the `./src/build/` directory called `FormatXML`.

### Version

You can set the version in the [`/src/version.txt`](/src/version.txt) file. The version is expected to be on the first line of this file. Each time you submit a PR, please update this version manually.

## License

![License](https://img.shields.io/github/license/richardjhughes/format-xml?style=plastic)
