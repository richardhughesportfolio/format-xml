use fxml::settings::config;
use std::io::Read;

fn main() {
    let args = std::env::args().collect();

    let config = config::Config::new(&args);

    let mut input = String::new();
    if let Err(e) = std::io::stdin().read_to_string(&mut input) {
        eprintln!("An error occurred when reading input for fxml: {e}");

        if config.strict() {
            std::process::exit(1);
        }
    }

    match fxml::run(&input, &config) {
        Ok(output) => print!("{output}"),
        Err(e) => {
            eprintln!("An error occurred when running fxml: {e}");
    
            if config.strict() {
                std::process::exit(1);
            }
        },
    }
}
