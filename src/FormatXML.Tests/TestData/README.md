# Test Data

Data needed for unit and integration tests are stored here.

The XML files in the `FormatSamples` directory are loaded automatically by tests. Each file is tested. These files must follow the naming scheme. Files with invalid names are ignored. If there are unmatched unformatted and formatted files, this will trigger a test failure.

Unformatted XML:

> SampleName_Unformatted.xml

Formatted XML:

> SampleName_Formatted.xml

The data are too big to store in the code directly. 
