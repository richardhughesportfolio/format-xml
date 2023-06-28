using FormatXML;

Application application = new(Environment.CommandLine, Console.In, Console.Out);
await application.Run();
