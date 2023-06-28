using FormatXML;

Application application = new(
    Environment.CommandLine,
    Console.In,
    Console.Out,
    Console.Error);
await application.Run();
