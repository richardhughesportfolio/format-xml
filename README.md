# format-xml

A command line utility to help format XML from `stdin` to `stdout`.

- [format-xml](#format-xml)
  - [Usage](#usage)
    - [Examples](#examples)

## Usage

The utility is called `fxml`. It takes XML via `stdin` and outputs the formatted `xml` via `stdout`. Errors are written to `stderr`.

Empty input results in empty output.

If the XML is invalid or cannot be formatted, an error will be written to `stderr` and the input will be written to `stdout`.

If an error occurs, `fxml` will return `0`. If you would like a non-zero value returned, you can pass the `--strict` flag.

### Examples

Basic usage:

```bash
cat file.xml | fxml > formatted.xml
```

To fail on an error:

```bash
cat invalid.xml | fxml --strict > formatted.xml
```

Note that `formatted.xml` will contain the contents of `invalid.xml`, because on error, the contents of `stdin` are outputted to `stdout`.

To see the help text:

```bash
fxml --help
```
